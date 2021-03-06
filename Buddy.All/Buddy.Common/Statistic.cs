﻿using Buddy.Common.Structures;
using System;
using System.Linq;

namespace Buddy.Common
{
    public class Statistic
    {
        private static double m_lastDistance;
        private static double m_lastCollision;

        public static void PrintStatistic(IGraph graph, double[] x, double[] y)
        {
            var distance = SumDistances(graph, x, y);
            var collision = CollisionArea(graph, x, y);

            Console.WriteLine("Статистика:");
            Console.WriteLine("Расстояние = {0:f} {1}", distance,
                m_lastDistance < distance ? '+' : Math.Abs(m_lastDistance - distance) < double.Epsilon ? ' ' : '-');
            Console.WriteLine("Площадь пересечений = {0:f} {1}", collision,
                m_lastCollision < collision ? '+' : Math.Abs(m_lastCollision - collision) < double.Epsilon ? ' ' : '-');

            m_lastDistance = distance;
            m_lastCollision = collision;
        }
        /// <summary>
        /// Расчёт суммарного расстояния
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double SumDistances(IGraph graph, double[] x, double[] y)
        {
            return
                graph.Vertices.Sum(
                    vertex => graph.SymAdj(vertex).Sum(second => Distance(x[vertex], y[vertex], x[second], y[second])));
        }
        /// <summary>
        /// Расчтё суммарной оласти пересечения
        /// </summary>
        /// <param name="graph">Граф</param>
        /// <param name="x">Координаты Х</param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double CollisionArea(IGraph graph, double[] x, double[] y)
        {
            return
                graph.Vertices.Sum(
                    vertex =>
                        graph.Adj(vertex)
                            .Sum(
                                second =>
                                    Collision(x[vertex], y[vertex], graph.Radiuses[vertex], x[second], y[second],
                                        graph.Radiuses[second])));
        }
        /// <summary>
        /// Расчёт площзади пересечения двух вершин
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="r1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        private static double Collision(double x1, double y1, double r1, double x2, double y2, double r2)
        {
            double R, r, d, d1, d2, A1, A2, s = 0;
            d = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            if (d <= Double.Epsilon)//центры совпадают
            {
                if (Math.Abs(r1 - r2) <= Double.Epsilon)
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
            if (d < r1 + r2)//вершины пересекаются
            {
                if (Math.Abs(r1 - r2) < Double.Epsilon)
                {
                    var cos = Math.Cos(d / (2 * r1));
                    if (cos < 0)
                    {
                        cos = -1 * cos;
                    }
                    s = 2 * r1 * r1 * Math.Pow(cos, -1) - d * Math.Sqrt(4 * r1 * r1 - d * d) / 2;
                }
                if (r1 > r2)
                {
                    R = r1;
                    r = r2;
                    d1 = (Math.Pow(d, 2) + Math.Pow(R, 2) - Math.Pow(r, 2)) / 2d;
                    d2 = (Math.Pow(d, 2) + Math.Pow(r, 2) - Math.Pow(R, 2)) / 2d;
                    var cos1 = Math.Cos(d2 / r);
                    var cos2 = Math.Cos(d1 / R);
                    if (cos1 < 0)
                    { cos1 = -1 * cos1; }
                    if (cos2 < 0)
                    { cos2 = -1 * cos2; }
                    A1 = Math.Pow(r, 2) * Math.Pow(cos1, -1);
                    A2 = Math.Pow(R, 2) * Math.Pow(cos2, -1);
                    s = A1 + A2 - (Math.Sqrt((-d + r - R) * (-d - r + R) * (-d + r + R) * (d + r + R))) / 2;
                }
                if (r2 > r1)
                {
                    R = r2;
                    r = r1;
                    d1 = (Math.Pow(d, 2) + Math.Pow(R, 2) - Math.Pow(r, 2)) / 2d;
                    d2 = (Math.Pow(d, 2) + Math.Pow(r, 2) - Math.Pow(R, 2)) / 2d;
                    var cos1 = Math.Cos(d2 / r);
                    var cos2 = Math.Cos(d1 / R);
                    if (cos1 < 0)
                    { cos1 = -1 * cos1; }
                    if (cos2 < 0)
                    { cos2 = -1 * cos2; }
                    A1 = Math.Pow(r, 2) * Math.Pow(cos1, -1);
                    A2 = Math.Pow(R, 2) * Math.Pow(cos2, -1);
                    s = A1 + A2 - (Math.Sqrt((-d + r + R) * (d + r - R) * (d - r + R) * (d + r + R))) / 2;
                }
                if (Math.Abs(d - r1 - r2) < Double.Epsilon)
                {
                    s = 0;
                }
            }
            if (double.IsNaN(s))
            {
                Console.WriteLine(@"Error in statistic. Input data x{0} y{1} r{2} and second x{3} y{4} r{5}",
                    x1, y1, r1, x2, y2, r2);
                return 0;
            }
            return s;
        }

        private static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
    }
}