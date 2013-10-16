using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Buddy.Common;

namespace Buddy.Placer
{
    public class ForceDirectedPlacer : BasePlacer
    {
        private static bool CrossingVertices(Vertex u, Vertex v, IList<PointF> coordinates)
        {
            return Distance(coordinates[u.Id], coordinates[v.Id]) <= u.Radius + v.Radius;
        }

        private static double Distance(PointF a, PointF b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        private static double Distance(PointF a)
        {
            return Math.Sqrt(Math.Pow(a.X, 2) + Math.Pow(a.Y, 2));
        }

        private static double AttractiveForce(Edge e)
        {
            return e.Weight;
        }

        private static double RepulsiveForce(Vertex u, Vertex v, IList<PointF> coordinates)
        {
            if (CrossingVertices(u, v, coordinates))
                return u.Radius + v.Radius - Distance(coordinates[u.Id], coordinates[v.Id]);
            return 0;
        }

        /// <summary>
        ///     Координаты вектора длины fourceModule исходящего из a в направлении к b
        /// </summary>
        /// <param name="a">первая точка</param>
        /// <param name="b">вторая точка</param>
        /// <param name="fourceModule">длина результирующего вектора</param>
        /// <returns></returns>
        private static PointF FindForceVector(PointF a, PointF b, double fourceModule)
        {
            var newCoord = new PointF
            {
                X = b.X - a.X, 
                Y = b.Y - a.Y
            };
            var x = (float)Math.Sqrt(Math.Pow(fourceModule, 2) / (1 + Math.Pow(newCoord.Y / newCoord.X, 2)));
            var vector1 = new PointF
            {
                X = x,
                Y = (newCoord.Y/newCoord.X)*x
            };
            var vector2 = new PointF
            {
                X = -vector1.X, 
                Y = -vector1.Y
            };
            return Distance(newCoord, vector1) < Distance(newCoord, vector2)
                ? vector1
                : vector2;
        }

        private static List<PointF> TotalVectorsOfForces(ISocialGraph graph, IList<PointF> coordinates)
        {
            var vectors = new List<PointF>();
            for (var i = 0; i < graph.Vertices.Count; i++)
            {
                vectors.Add(new PointF(0, 0));
            }
            foreach (var e in graph.Edges)
            {
                var u = FindForceVector(coordinates[e.U.Id], coordinates[e.V.Id], AttractiveForce(e));
                var a = vectors[e.U.Id];
                a.X += u.X;
                a.Y += u.Y;
                vectors[e.U.Id] = a;

                var b = vectors[e.V.Id];
                b.X -= u.X;
                b.Y -= u.Y;
                vectors[e.V.Id] = b;
            }

            for (var i = 0; i < graph.Vertices.Count; i++)
            {
                for (var j = i + 1; j < graph.Vertices.Count; j++)
                {
                    var repulsiveForce = RepulsiveForce(graph.Vertices[i], graph.Vertices[j], coordinates);
                    if (repulsiveForce > 0)
                    {
                        var u = FindForceVector(coordinates[i], coordinates[j], repulsiveForce);
                        vectors[i] = new PointF(vectors[i].X - u.X, vectors[i].Y - u.Y);
                        vectors[j] = new PointF(vectors[j].X + u.X, vectors[j].Y + u.Y);
                    }
                }
            }
            return vectors;
        }

        private static double MaxStep(Size size, ISocialGraph graph)
        {
            var maxRadus = graph.Vertices.Max(v => v.Radius);
            var maxStep = Math.Sqrt(Math.Pow(size.Width, 2) + Math.Pow(size.Height, 2))/maxRadus;
            return maxStep;
        }

        private static double ReductionCoef(Size size, ISocialGraph graph, IEnumerable<PointF> vectors)
        {
            var maxModule = vectors.Max(v => Distance(v));
            return MaxStep(size, graph)/maxModule;
        }

        public override IList<PointF> PlaceGraph(ISocialGraph graph, IList<PointF> coordinates, Size size)
        {
            var newCoord = coordinates.ToList();

            do
            {
                var totoalVectors = TotalVectorsOfForces(graph, coordinates);
                var reductCoef = ReductionCoef(size, graph, totoalVectors);
                for (var i = 0; i < graph.Vertices.Count; i++)
                {
                    var newPosition = FindForceVector(new PointF(0, 0), totoalVectors[i],
                        Distance(totoalVectors[i])*reductCoef);
                    newPosition.X = newPosition.X + newCoord[i].X;
                    newPosition.Y = newPosition.Y + newCoord[i].Y;
                    newCoord[i] = newPosition;
                }
                Iterations--;
            } while (Iterations > 0);
            return newCoord;
        }
    }
}