using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Buddy.Common.Structures;

namespace Buddy.Placer
{
   public class ForceDirectedCSR : BasePlacer
    {
        public ForceDirectedCSR(ISettings settings) : base(settings)
        {
            
        }


       private static double Distance(Coordinate a, Coordinate b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        private static double Norma(Coordinate a)
        {
            return Math.Sqrt(Math.Pow(a.X, 2) + Math.Pow(a.Y, 2));
        }

       
        private static Coordinate FindForceVector(Coordinate a, Coordinate b, double fourceModule)
        {
            Coordinate newCoord;
            if ((Math.Abs(a.X - b.X) <double.Epsilon) && (Math.Abs(a.Y - b.Y) < double.Epsilon))
            {
                Random r = new Random();
                 newCoord = new Coordinate
                {
                    X = r.NextDouble() ,
                    Y = r.NextDouble() 
                };

            }
            else
            {
                newCoord = new Coordinate
                {
                    X = b.X - a.X,
                    Y = b.Y - a.Y
                };
            }
            var norma = Norma(newCoord);
            newCoord.X = (newCoord.X / norma) * fourceModule;
            newCoord.Y = (newCoord.Y / norma) * fourceModule;

            return newCoord;
        }


        private static void CulcAttractiveForces(IGraph graph, IList<Coordinate> coordinates,
            IList<Coordinate> vectors)
        {
            for (var i = 0; i < graph.XAdj.Length-1; i++)
            {
                for (var k = graph.XAdj[i]; k < graph.XAdj[i+1]; k++)
                {
                    
                    var j = graph.Adjency[k];
                    if (j < i) continue;
                    var AttrForce = Distance(coordinates[i], coordinates[j]) * graph.Weights[k];
                    var u = FindForceVector(coordinates[i], coordinates[j], AttrForce);
                    vectors[i].X += u.X;
                    vectors[i].Y += u.Y;
                    vectors[j].X -= u.X;
                    vectors[j].Y -= u.Y;
                }
            }
        }

        private static void CulcRepulsiveForces(IGraph graph, IList<Coordinate> coordinates,
            IList<Coordinate> vectors, Size size)
        {
           // INeighbor g = new NeighborTester.NeighborGraph(graph, size, coordinates);
            double RepFoce;
            for (var i = 0; i < graph.VerticesAmount; i++)
            {//  var neighbors = g.Neighborhood(coordinates[i], i);
             //  var etr = neighbors.GetEnumerator;
               for (var j = i + 1; j < graph.VerticesAmount; j++)
            //   while(etr.MoveNext())
                {
                   // var j = etr.Current;
                    if (Math.Abs(coordinates[i].X - coordinates[j].X) < double.Epsilon && Math.Abs(coordinates[i].Y - coordinates[j].Y) < double.Epsilon)
                        RepFoce = -(graph.Radiuses[i]+graph.Radiuses[j]);
                    else RepFoce = -1 / Distance(coordinates[i], coordinates[j]);
                    var u = FindForceVector(coordinates[i], coordinates[j], RepFoce);
                    vectors[i].X += u.X;
                    vectors[i].Y += u.Y;
                    vectors[j].X -= u.X;
                    vectors[j].Y -= u.Y;

                }
            }
        }

        private static double MaxStep(Size size, IGraph graph)
        {
            return 10;
        }

        private static double ReductionCoef(Size size, IGraph graph, IList<Coordinate> vectors)
        {
            var maxModule = vectors.Max(v => Norma(v));
            return MaxStep(size, graph) / maxModule;
        }

        private static void CulcGradient(IGraph graph, IList<Coordinate> coordinates, Coordinate[] gradient, Size size)
        {
            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                gradient[i].X = 0;
                gradient[i].Y = 0;
            }
            CulcAttractiveForces(graph, coordinates, gradient);
            CulcRepulsiveForces(graph, coordinates, gradient, size);
            var r1 = ReductionCoef(size, graph, gradient);
            foreach (var v in gradient)
            {
                v.X = v.X * r1;
                v.Y = v.Y * r1;
            }
        }

        public static void Scale(Size size, IList<Coordinate> coordinates, IGraph graph)
        {
            return;
            var maxX = coordinates[0].X;
            var minX = coordinates[0].Y;
            var maxY = coordinates[0].X;
            var minY = coordinates[0].Y;

            for (var i = 1; i < coordinates.Count; i++)
            {
                if (coordinates[i].X > maxX) maxX = coordinates[i].X;
                if (coordinates[i].Y > maxY) maxY = coordinates[i].Y;
                if (coordinates[i].X < minX) minX = coordinates[i].X;
                if (coordinates[i].Y < minY) minY = coordinates[i].Y;

            }

            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                if (coordinates[i].X + graph.Radiuses[i] > maxX) maxX = coordinates[i].X + graph.Radiuses[i];
                if (coordinates[i].Y + graph.Radiuses[i] > maxY) maxY = coordinates[i].Y + graph.Radiuses[i];
                if (coordinates[i].X - graph.Radiuses[i] < minX) minX = coordinates[i].X - graph.Radiuses[i];
                if (coordinates[i].Y - graph.Radiuses[i] < minY) minY = coordinates[i].Y - graph.Radiuses[i];
            }
            if ((maxX > size.Width) || (maxY > size.Height) || (minX < 0) || (minY < 0))
            {
                var weight = maxX - minX;
                var height = maxY - minY;

                var coeffx = (size.Width - 20) / weight;
                var coeffy = (size.Height - 20) / height;
                for (var i = 0; i < graph.VerticesAmount; i++)
                {
                    coordinates[i].X = (coordinates[i].X - minX) * coeffx + 10;
                    coordinates[i].Y = (coordinates[i].Y - minY) * coeffy + 10;

                }
            }

        }


        public override IList<Coordinate> PlaceGraph(IGraph graph, IList<Coordinate> coordinate, Size size)
        {
            IList<Coordinate> newCoord = coordinate.ToList();
            var iterations = Settings.Iterations;
            Coordinate[] gradient = new Coordinate[graph.VerticesAmount];
            for (var i = 0; i < gradient.Length; i++)
                gradient[i] = new Coordinate(0, 0);
            do
            {
                CulcGradient(graph, newCoord, gradient, size);

                for (var i = 0; i < graph.VerticesAmount; i++)
                {
                    newCoord[i].X += gradient[i].X;
                    newCoord[i].Y += gradient[i].Y;
                }
                iterations--;
                Scale(size, newCoord, graph);
            } while (iterations > 0);

            return newCoord;
        }
    }
}
