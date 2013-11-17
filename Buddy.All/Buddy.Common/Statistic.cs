using Buddy.Common.Structures;
using System;

namespace Buddy.Common
{
    public class Statistic
    {
        private static double _lastDistance;
        private static double _lastCollision;

        public static void PrintStatistic(IGraph graph, double[] x, double[] y)
        {
            var distance = SumDistances(graph, x, y);
            var collision = CollisionArea(graph, x, y);

            Console.WriteLine("Statistic:");
            Console.WriteLine("Distance = {0:f} {1}", distance,
                _lastDistance < distance ? '+' : Math.Abs(_lastDistance - distance) < double.Epsilon ? ' ' : '-');
            Console.WriteLine("Collision = {0:f} {1}", collision,
                _lastCollision < collision ? '+' : Math.Abs(_lastCollision - collision) < double.Epsilon ? ' ' : '-');

            _lastDistance = distance;
            _lastCollision = collision;
        }

        public static double SumDistances(IGraph graph, double[] x, double[] y)
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

        public static double CollisionArea(IGraph graph, double[] x, double[] y)
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
            double R,r,d, d1, d2,A1,A2, s = 0;
            d = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            if (d == 0)//центры совпадают
            {
                if (r1 == r2)
                {
                    s = Math.PI * r1 * r1;
                }
                if (r1 > r2)
                {
                    s = Math.PI * r2 * r2;
                }
                if (r2 > r1)
                {
                    s = Math.PI * r1 * r1;
                }
            }
            if (d > r1 + r2)//вершины далеко друг от друга
            {
                s = 0;
            }
            if (d< r1+r2)//вершины пересекаются
            {
                if (r1 == r2)
                {
                    double cos = Math.Cos(d /( 2 * r1));
                    if (cos < 0)
                    {
                        cos = -1*cos;
                    }
                    s = 2 * r1 * r1 * Math.Pow(cos, -1) -  d * Math.Sqrt(4 * r1 * r1 - d * d)/2;
                }
                if (r1 > r2)
                {
                    R = r1;
                    r = r2;
                    d1 = (Math.Pow(d, 2) + Math.Pow(R, 2) - Math.Pow(r, 2)) / 2d;
                    d2 = (Math.Pow(d, 2) + Math.Pow(r, 2) - Math.Pow(R, 2)) / 2d;
                    double cos1 = Math.Cos(d2 / r);
                    double cos2 = Math.Cos(d1 / R);
                    if (cos1 < 0)
                    { cos1 =-1*cos1;}
                    if (cos2 < 0)
                    { cos2 = -1 * cos2; }
                    A1 = Math.Pow(r, 2) * Math.Pow(cos1, -1);
                    A2 = Math.Pow(R, 2) * Math.Pow(cos2, -1);
                    s = A1 + A2 - (Math.Sqrt((-d + r - R) * (-d - r + R) * (-d + r + R) * (d + r + R)))/2;
                }
                if (r2 > r1)
                {
                    R = r2;
                    r = r1;
                    d1 = (Math.Pow(d, 2) + Math.Pow(R, 2) - Math.Pow(r, 2)) / 2d;
                    d2 = (Math.Pow(d, 2) + Math.Pow(r, 2) - Math.Pow(R, 2)) / 2d;
                    double cos1 = Math.Cos(d2 / r);
                    double cos2 = Math.Cos(d1 / R);
                    if (cos1 < 0)
                    { cos1 = -1 * cos1; }
                    if (cos2 < 0)
                    { cos2 = -1 * cos2; }
                    A1 = Math.Pow(r, 2) * Math.Pow(cos1, -1);
                    A2 = Math.Pow(R, 2) * Math.Pow(cos2, -1);
                    s = A1 + A2 - (Math.Sqrt((-d + r - R) * (-d - r + R) * (-d + r + R) * (d + r + R)))/2;
                }
                if (d == r1 + r2)
                {
                    s = 0;
                }
               
            }
          
            return s;
        }

        private static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
    }
}