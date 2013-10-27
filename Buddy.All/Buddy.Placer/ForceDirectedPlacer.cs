using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Buddy.Common.Structures;

namespace Buddy.Placer
{
    public class ForceDirectedPlacer : BasePlacer
    {
        public ForceDirectedPlacer(ISettings settings)
            : base(settings)
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

        private static double AttractiveForce(Edge e, IList<Coordinate> coordinates)
        {
            return Distance(coordinates[e.U.Id], coordinates[e.V.Id]) * e.Weight;
        }

        private static double RepulsiveForce(Vertex u, Vertex v, IList<Coordinate> coordinates)
        {
            return -1 / Distance(coordinates[u.Id], coordinates[v.Id]);
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

        private static void CulcAttractiveForces(ISocialGraph graph, IList<Coordinate> coordinates,
             List<Coordinate> vectors)
        {
            foreach (var e in graph.Edges)
            {
                var u = FindForceVector(coordinates[e.U.Id], coordinates[e.V.Id],
                    AttractiveForce(e, coordinates));

                vectors[e.U.Id].X += u.X;
                vectors[e.U.Id].Y += u.Y;

                vectors[e.V.Id].X -= u.X;
                vectors[e.V.Id].Y -= u.Y;

            }
        }

        private static void CulcRepulsiveForces(ISocialGraph graph, IList<Coordinate> coordinates,
             List<Coordinate> vectors)
        {
            for (var i = 0; i < graph.Vertices.Count; i++)
            {
                for (var j = i + 1; j < graph.Vertices.Count; j++)
                {
                    var repulsiveForce = RepulsiveForce(graph.Vertices[i], graph.Vertices[j], coordinates);

                    var u = FindForceVector(coordinates[i], coordinates[j], repulsiveForce);
                    vectors[i].X += u.X;
                    vectors[i].Y += u.Y;
                    vectors[j].X -= u.X;
                    vectors[j].Y -= u.Y;

                }
            }
        }

        private static List<Coordinate> CulcGradient(ISocialGraph graph, IList<Coordinate> coordinates)
        {
            var vectors = new List<Coordinate>();
            for (var i = 0; i < graph.Vertices.Count; i++)
            {
                vectors.Add(new Coordinate(0, 0));
            }
            CulcAttractiveForces(graph, coordinates, vectors);// закоментировать, чтобы получить 2-ой подход
            CulcRepulsiveForces(graph, coordinates, vectors);//закоментировать, чтобы получить 1-ый подход

            return vectors;
        }

        private static double MaxStep(Size size, ISocialGraph graph)
        {
            var maxRadus = graph.Vertices.Max(v => v.Radius);
            var maxStep = Math.Sqrt(Math.Pow(size.Width, 2) + Math.Pow(size.Height, 2)) / maxRadus;
            return maxStep;
        }

        private static double ReductionCoef(Size size, ISocialGraph graph, IList<Coordinate> vectors)
        {
            var maxModule = vectors.Max(v => Norma(v));
            return MaxStep(size, graph) / maxModule;
        }

        public static void Scale(Size size, ref IList<Coordinate> coordinates)
        {
            var maxX = coordinates.Max(c => c.X);
            var minX = coordinates.Min(c => c.X);
            var maxY = coordinates.Max(c => c.Y);
            var minY = coordinates.Min(c => c.Y);

            var weight = maxX - minX;
            var height = maxY - minY;

            var coeff = Math.Min((size.Width - 20) / weight, (size.Height - 20) / height);
            for (var i = 0; i < coordinates.Count; i++)
            {
                var newCoord = new Coordinate
                {
                    X = (coordinates[i].X - minX) * coeff + 10,
                    Y = (coordinates[i].Y - minY) * coeff + 10
                };
                coordinates[i] = newCoord;
            }

        }

        public override IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, Size size)
        {
            IList<Coordinate> newCoord = coordinates.ToList();
            var iterations = Settings.Iterations;

            do
            {
                var gradient = CulcGradient(graph, newCoord);
                var reductCoef = ReductionCoef(size, graph, gradient);
                for (var i = 0; i < graph.Vertices.Count; i++)
                {
                    var newPosition = new Coordinate(gradient[i].X * reductCoef, gradient[i].Y * reductCoef); //FindForceVector(new PointF(0, 0), totoalVectors[i],
                    newPosition.X = newPosition.X + newCoord[i].X;
                    newPosition.Y = newPosition.Y + newCoord[i].Y;
                    newCoord[i] = newPosition;

                }
                iterations--;
            } while (iterations > 0);
            Scale(size, ref newCoord);
            return newCoord;
        }
    }
}