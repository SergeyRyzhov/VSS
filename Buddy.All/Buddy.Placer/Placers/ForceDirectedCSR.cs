﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Buddy.Common.Printers;
using Buddy.Common.Structures;

namespace Buddy.Placer.Placers
{
    public class ForceDirectedCSR : BasePlacer
    {
        private static Random m_random;

        public ForceDirectedCSR(ISettings settings)
            : base(settings)
        {
            m_random = new Random();
        }

        private static double Distance(double ax, double ay, double bx, double by)
        {
            return Math.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay));
        }

        private static double Norma(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        private static void FindForceVector(double ax, double ay, double bx, double by, double fourceModule,
            out double x,
            out double y)
        {
            //todo может быть для каждой координаты отдельное сравнение?
            if ((Math.Abs(ax - bx) < double.Epsilon) && (Math.Abs(ay - by) < double.Epsilon))
            {
                x = m_random.NextDouble();
                y = m_random.NextDouble();
            }
            else
            {
                x = bx - ax;
                y = by - ay;
            }

            var norma = Norma(x, y);
            norma = norma > double.Epsilon ? norma : 1;

            x = x > double.Epsilon ? (x / norma) * fourceModule : 0;
            y = y > double.Epsilon ? (y / norma) * fourceModule : 0;
        }

        private static void CulcAttractiveForces(IGraph graph, IList<double> inX, IList<double> inY,
            ref double[] outX, ref double[] outY)
        {
            foreach (var v in graph.Vertices)
            {
                //todo нужно ли сделать сокрытие внутренних структур
                //foreach (var u in graph.SymAdj(v))
                for (var k = graph.XAdj[v]; k < graph.XAdj[v + 1]; k++)
                {
                    var u = graph.Adjency[k];

                    if (u <= v) continue;

                    var attrForce = Distance(inX[v], inY[v], inX[u], inY[u])*graph.Weights[k];
                    double x;
                    double y;

                    FindForceVector(inX[v], inY[v], inX[u], inY[u], attrForce, out x, out y);
                    if (double.IsNaN(x))
                        throw new Exception();
                    if (double.IsNaN(y))
                        throw new Exception();

                    outX[v] += x;
                    outY[v] += y;
                    outX[u] -= x;
                    outY[u] -= y;
                }
            }
        }

        private static void CulcRepulsiveForces(IGraph graph, IList<double> inX, IList<double> inY,
            ref double[] outX, ref double[] outY)
        {
            //todo вынести логику получения соседей в отдельный интерфейс/класс
            for (var v = 0; v < graph.VerticesAmount; v++)
            {
                for (var u = v + 1; u < graph.VerticesAmount; u++)
                {
                    double repFoce;
                    if (Math.Abs(inX[v] - inX[u]) < double.Epsilon &&
                        Math.Abs(inY[v] - inY[u]) < double.Epsilon)
                    {
                        repFoce = -(graph.Radius(v) + graph.Radius(u));
                    }
                    else
                    {
                        repFoce = -(graph.Radius(v) + graph.Radius(u)) / Distance(inX[v], inY[v], inX[u], inY[u]);
                    }

                    double x;
                    double y;

                    FindForceVector(inX[v], inY[v], inX[u], inY[u], repFoce, out x, out y);
                    outX[v] += x;
                    outY[v] += y;
                    outX[u] -= x;
                    outY[u] -= y;
                }
            }
        }

        private static double MaxStep
        {
            //todo вынести в настройки, либо рассчитывать
            get
            {
                return 10;
            }
        }

        private static double ReductionCoef(IList<double> x, IList<double> y)
        {
            double maxModule = 0;
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < x.Count; i++)
            {
                maxModule = Math.Max(maxModule, Norma(x[i], y[i]));
                if (double.IsNaN(maxModule))
                    throw new Exception();
            }
            return MaxStep / maxModule;
        }

        private static void CulcGradient(IGraph graph, IList<double> inX, IList<double> inY, ref double[] outX, ref double[] outY)
        {
            foreach (var vertex in graph.Vertices)
            {
                outX[vertex] = 0;
                outY[vertex] = 0;
            }

            CulcAttractiveForces(graph, inX, inY, ref outX, ref outY);
            for (int i = 0; i < outX.Length; i++)
            {
                if (double.IsNaN(outX[i]))
                    throw new Exception();
            }

            CulcRepulsiveForces(graph, inX, inY, ref outX, ref outY);
            for (int i = 0; i < outX.Length; i++)
            {
                if (double.IsNaN(outX[i]))
                    throw new Exception();
            }

            var reductionCoef = ReductionCoef(outX, outY);
            foreach (var vertex in graph.Vertices)
            {
                outX[vertex] *= reductionCoef;
                outY[vertex] *= reductionCoef;
            }
        }

        //todo исправить если есть ошибка
        public static void Scale(IGraph graph, Size size, ref double[] x, ref double[] y, bool always = false)
        {
            var maxX = x[0];
            var minX = x[0];

            var maxY = y[0];
            var minY = y[0];

            foreach (var vertex in graph.Vertices)
            {
                maxX = Math.Max(maxX, x[vertex]);
                minX = Math.Min(minX, x[vertex]);

                maxY = Math.Max(maxY, y[vertex]);
                minY = Math.Min(minY, y[vertex]);
            }

            foreach (var vertex in graph.Vertices)
            {
                maxX = Math.Max(maxX, x[vertex] + graph.Radius(vertex));
                minX = Math.Min(minX, x[vertex] - graph.Radius(vertex));

                maxY = Math.Max(maxY, y[vertex] + graph.Radius(vertex));
                minY = Math.Min(minY, y[vertex] - graph.Radius(vertex));
            }

            if ((maxX > size.Width) || (maxY > size.Height) || (minX < 0) || (minY < 0) || always)
            {
                var weight = maxX - minX;
                var height = maxY - minY;

                //todo вынести если нужно в настройки
                const int limit = 20;
                var coeffX = (size.Width - limit) / weight;
                var coeffY = (size.Height - limit) / height;

                //todo вынести если нужно в настройки
                const int limitBorder = 10;
                foreach (var vertex in graph.Vertices)
                {
                    x[vertex] = (x[vertex] - minX) * coeffX + limitBorder;
                    y[vertex] = (y[vertex] - minY) * coeffY + limitBorder;
                }
            }

            foreach (var vertex in graph.Vertices)
            {
                if (x[vertex] > size.Width)
                    throw new Exception();

                if (y[vertex] > size.Height)
                    throw new Exception();
            }
        }

        public override void PlaceGraph(IGraph graph, Size size, double[] inX, double[] inY, ref double[] outX, ref double[] outY)
        {
            var iterations = Settings.Iterations;

            Array.Copy(inX, outX, inX.Length);
            Array.Copy(inY, outY, inY.Length);

            var x = new double[inX.Length];
            var y = new double[inY.Length];

            foreach (var vertex in graph.Vertices)
            {
                if (double.IsNaN(outX[vertex]))
                    throw new Exception();
            }
            Drawer.Pause();
            while (iterations-- > 0)
            {
                CulcGradient(graph, outX, outY, ref x, ref y);

                foreach (var vertex in graph.Vertices)
                {
                    if (double.IsNaN(x[vertex]))
                        throw new Exception();

                    outX[vertex] += x[vertex];
                    outY[vertex] += y[vertex];
                }

                Drawer.DrawGraph(size, graph, outX, outY, string.Format("force_directed_{0}.bmp", Settings.Iterations - iterations));
            }
            Scale(graph, size, ref outX, ref outY);
            Drawer.Resume();
        }
    }
}