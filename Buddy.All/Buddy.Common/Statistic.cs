using System;
using Buddy.Common.Structures;

namespace Buddy.Common
{
    public class Statistic
    {
        public double SumDistances(IGraph graph, double[] x, double[] y)
        {
            var distance = 0.0;
            for (uint i = 0; i < graph.VerticesAmount; i++)
            {
                for (uint j = graph.RowIndex[i]; j < graph.RowIndex[i + 1]; j++)
                {
                    var first = graph.ColumnIndex[j];
                    var second = i;

                    distance += Distance(x[first], y[first], x[second], y[second]);
                }
            }

            return distance;
        }

        public double CollisionArea(IGraph graph, double[] x, double[] y)
        {
            var distance = 0.0;
            for (uint i = 0; i < graph.VerticesAmount; i++)
            {
                for (uint j = graph.RowIndex[i]; j < graph.RowIndex[i + 1]; j++)
                {
                    var first = graph.ColumnIndex[j];
                    var second = i;

                    distance += Collision(x[first], y[first], graph.Radius[first], x[second], y[second], graph.Radius[second]);
                }
            }

            return distance;
        }

        private static double Collision(double x1, double y1, double r1, double x2, double y2, double r2)
        {
            //TODO реализовать расчёт площади пересечения кругов
            return Math.PI*r1*r1 + Math.PI*r2*r2;
            double f1, f2,d,s1,s2,s;
            d = Math.Sqrt(Math.Pow(x2-x1,2)+Math.Pow(y2-y1,2));
            if (d == r1)
            {
                s = Math.PI * r1 * r1;
            }
            if (d == r2)
            {
                s = Math.PI * r2 * r2;
            }
            if (d < r1+r2)
            {
                f2 = 2 * Math.Acos((Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(d, 2)) / (2 * r1 * d));
                f1 = 2 * Math.Acos((Math.Pow(r2, 2) - Math.Pow(r1, 2) + Math.Pow(d, 2)) / (2 * r2 * d));
                s1 = Math.Pow(r1, 2) * (f1 - Math.Sin(f1)) / 2;
                s2 = Math.Pow(r2, 2) * (f2 - Math.Sin(f2)) / 2;
                s = s1 + s2;
            }
        }

        private static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));
        }
    }
}
