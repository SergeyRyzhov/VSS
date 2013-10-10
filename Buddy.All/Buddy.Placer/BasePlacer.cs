using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy.Common;
using System.Drawing;
using System.Windows;


namespace Buddy.Placer
{
    class BasePlacer
    {
        int iterations;

        int Iterations
        {
            set { this.iterations = value; }
        }
        bool Crossing_Vertices(Vertex v1, Vertex v2, IList<PointF> coordinates)
        {
            if (Distance(coordinates[(int)v1.Id], coordinates[(int)v2.Id]) <= v1.Radius + v2.Radius)
                return true;
            else return false;
        }

        double Distance(PointF p1, PointF p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));

        }
        double Distance(PointF p1)
        {
            return Math.Sqrt(Math.Pow(p1.X, 2) + Math.Pow(p1.Y, 2));
        }

        double AttractiveForce(Edge e)
        {
            return e.Weight;
        }

        double RepulsiveForce(Vertex v1, Vertex v2, IList<PointF> coordinates)
        {
            if (Crossing_Vertices(v1, v2, coordinates)) return v1.Radius + v2.Radius - Distance(coordinates[(int)v1.Id], coordinates[(int)v2.Id]);
            else return 0;          
        }

        PointF Find_force_vector(PointF p1, PointF p2, double fource_module)//координаты вектора длины fource_module исходящего из p1 в направлении к p2
        {
            PointF new_coord = new PointF(), vector1 = new PointF(), vector2 = new PointF();
            new_coord.X = p2.X - p1.X;
            new_coord.Y = p2.Y - p1.Y;
            vector1.X = (float)Math.Sqrt(Math.Pow(fource_module, 2) / (1 + Math.Pow(new_coord.Y / new_coord.X, 2)));
            vector1.Y = (new_coord.Y / new_coord.X) * vector1.X;
            vector2.X = -1 * vector1.X;
            vector2.Y = -1 * vector1.Y;
            if (Distance(new_coord, vector1) < Distance(new_coord, vector2)) return vector1;
            else return vector2;
        }

        List<PointF> Total_vectors_of_forces(ISotialGraph graph, IList<PointF> coordinates)
        {
            List<PointF> vectors = new List<PointF>();
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                vectors.Add(new PointF(0, 0));
            }
            foreach (Edge e in graph.Edges)
            {
                PointF u = Find_force_vector(coordinates[(int)e.U.Id], coordinates[(int)e.V.Id], AttractiveForce(e));
                vectors[(int)e.U.Id] = new PointF(vectors[(int)e.U.Id].X + u.X, vectors[(int)e.U.Id].Y + u.Y);
                vectors[(int)e.V.Id] = new PointF(vectors[(int)e.V.Id].X - u.X, vectors[(int)e.V.Id].Y - u.Y);
            }

            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                for (int j = i + 1; j < graph.Vertices.Count; j++)
                {
                    if (RepulsiveForce(graph.Vertices[i], graph.Vertices[j], coordinates) > 0)
                    {
                        PointF u = Find_force_vector(coordinates[i], coordinates[j], RepulsiveForce(graph.Vertices[i], graph.Vertices[j], coordinates));
                        vectors[i] = new PointF(vectors[i].X - u.X, vectors[i].Y - u.Y);
                        vectors[j] = new PointF(vectors[j].X + u.X, vectors[j].Y + u.Y);
                    }
                }
            }
            return vectors;
        }


        double Max_step(Size size, ISotialGraph graph)
        {
            double max_step, max_radus = 0;
            foreach (Vertex v in graph.Vertices)
            {
                if (v.Radius > max_radus) max_radus = v.Radius;
            }
            max_step = Math.Sqrt(Math.Pow(size.Width, 2) + Math.Pow(size.Height, 2)) / max_radus;
            return max_step;

        }

        double Reduction_coef(Size size, ISotialGraph graph, IList<PointF> vectors)
        {
            double max_module = 0;
            foreach (PointF p in vectors)
            {
                if (Distance(p) > max_module) max_module = Distance(p);
            }
            return Max_step(size, graph) / max_module;
        }

        public IList<PointF> PlaceGraph(ISotialGraph graph, IList<PointF> coordinates, Size size)
        {
            IList<PointF> new_coord = coordinates;
            PointF new_position;
            List<PointF> totoal_vectors;
            double reduct_coef;

            do
            {
                totoal_vectors = Total_vectors_of_forces(graph, new_coord);
                reduct_coef = Reduction_coef(size, graph, totoal_vectors);
                for (int i = 0; i < graph.Vertices.Count; i++)
                {
                    new_position = Find_force_vector(new PointF(0, 0), totoal_vectors[i], Distance(totoal_vectors[i]) * reduct_coef);
                    new_position.X = new_position.X + new_coord[i].X;
                    new_position.Y = new_position.Y + new_coord[i].Y;
                    new_coord[i] = new_position;
                }
                iterations--;
            } while (iterations > 0);
            return new_coord;

        }
    }
}
