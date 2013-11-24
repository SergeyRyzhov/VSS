using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Buddy.Common.Structures;

namespace Buddy.Placer
{
   public class ForceDirectedCSR:BasePlacer
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

        private static double AttractiveForce(Coordinate a, Coordinate b, double weight)
        {
            return Distance(a, b)*weight;
        }

        private static double RepulsiveForce(Coordinate a, Coordinate b,double radiusA,double radiusB)
        {
            return -(radiusA+radiusB)/Distance(a, b);
        }

        private static Coordinate FindForceVector(Coordinate a, Coordinate b, double fourceModule)
        {
            var newCoord = new Coordinate
            {
                X = b.X - a.X,
                Y = b.Y - a.Y
            };
            var norma = Norma(newCoord);
            newCoord.X = (newCoord.X / norma) * fourceModule;
            newCoord.Y = (newCoord.Y / norma) * fourceModule;

            return newCoord;
        }

      
        private static void CulcAttractiveForces(IGraph graph, IList<Coordinate> coordinates,
            IList<Coordinate> vectors)
        {
            for (var i = 0; i < graph.RowIndex.Length-2; i++)
            {
                for (var k = graph.RowIndex[i]; k < graph.RowIndex[i+1]; k++)
                {
                    var j =(int) graph.ColumnIndex[k];
                    var u = FindForceVector(coordinates[i], coordinates[j],
                    AttractiveForce(coordinates[i], coordinates[j], graph.Weight[k]));
                    vectors[i].X += u.X;
                    vectors[i].Y += u.Y;
                    vectors[j].X -= u.X;
                    vectors[j].Y -= u.Y;
                }
            }
        }

        private static void CulcRepulsiveForces(IGraph graph, IList<Coordinate> coordinates,
            IList<Coordinate> vectors)
        { 
            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                for (var j = i + 1; j < graph.VerticesAmount; j++)
                {
                    var repulsiveForce = RepulsiveForce(coordinates[i], coordinates[j],graph.Radius[i],graph.Radius[j]);

                    var u = FindForceVector(coordinates[i], coordinates[j], repulsiveForce);
                    vectors[i].X += u.X;
                    vectors[i].Y += u.Y;
                    vectors[j].X -= u.X;
                    vectors[j].Y -= u.Y;

                }
            }
        }

        private static double MaxStep(Size size, IGraph graph)
        {
            //var maxRadus = graph.Radius.Max();
            //var maxStep = Math.Sqrt(Math.Pow(size.Width, 2) + Math.Pow(size.Height, 2)) / maxRadus;
            return 10;
        }

        private static double ReductionCoef(Size size, IGraph graph, IList<Coordinate> vectors)
        {
            var maxModule = vectors.Max(v => Norma(v));
            return MaxStep(size, graph) / maxModule;
        }

        private static List<Coordinate> CulcGradient(IGraph graph, IList<Coordinate> coordinates, Size size)
        {
            var vectors1 = new List<Coordinate>();
            var vectors2 = new List<Coordinate>();

            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                vectors1.Add(new Coordinate(0, 0));
                vectors2.Add(new Coordinate(0, 0));
            }
            CulcAttractiveForces(graph, coordinates, vectors1);
            CulcRepulsiveForces(graph, coordinates, vectors2);

            var r1 = ReductionCoef(size, graph, vectors1);
            var r2 = ReductionCoef(size, graph, vectors2);
            foreach (var v in vectors1)
            {
                v.X = v.X * r1;
                v.Y = v.Y * r1;
            }
            foreach (var v in vectors2)
            {
                v.X = v.X * r2;
                v.Y = v.Y * r2;
            }
            for (var i = 0; i < vectors1.Count; i++)
            {
                vectors1[i].X += vectors2[i].X;
                vectors1[i].Y += vectors2[i].Y;
            }

            return vectors1;
        }

        public static void Scale(Size size, IList<Coordinate> coordinates, IGraph graph)
        {

            var maxX = coordinates.Max(c => c.X);
            var minX = coordinates.Min(c => c.X);
            var maxY = coordinates.Max(c => c.Y);
            var minY = coordinates.Min(c => c.Y);

            var weight = maxX - minX;
            var height = maxY - minY;

            var coeffx = (size.Width - 20) / weight;
            var coeffy = (size.Height - 20) / height;
            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                var newCoord = new Coordinate
                {
                    X = (coordinates[i].X - minX) * coeffx + 10,
                    Y = (coordinates[i].Y - minY) * coeffy + 10
                };
                coordinates[i] = newCoord;
            }
        }


         public override IList<Coordinate> PlaceGraph(IGraph graph, IList<Coordinate> coordinate, Size size)
        {
            IList<Coordinate> newCoord = coordinate.ToList();
            var iterations = Settings.Iterations;

            do
            {
                var gradient = CulcGradient(graph, newCoord, size);

                for (var i = 0; i < graph.VerticesAmount; i++)
                {
                    newCoord[i].X += gradient[i].X;
                    newCoord[i].Y += gradient[i].Y;
                }
                iterations--;
                Scale(size, newCoord,graph);
            } while (iterations > 0);
            return newCoord;
        }
    }
}
