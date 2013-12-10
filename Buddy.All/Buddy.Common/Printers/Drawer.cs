using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Buddy.Common.Structures;

namespace Buddy.Common.Printers
{
    public class Drawer
    {
        private static int m_number;

        private static string m_firstFile;

        private static bool m_skip;

        private static bool m_globalSkip;

        static Drawer()
        {
            m_skip = false;
        }

        public static void Pause()
        {
            m_skip = true;
        }

        public static void Resume()
        {
            m_skip = false;
        }

        public static void GlobalPause()
        {
            m_globalSkip = true;
            m_skip = true;
        }

        public static void GlobalResume()
        {
            m_globalSkip = false;
            m_skip = false;
        }

        public static void DrawGraph(Size size, IGraph graph, double[] cX, double[] cY, string fileName)
        {
            if (m_skip || m_globalSkip)
                return;

            var vertexBrush = new SolidBrush(Color.FromArgb(32, Color.DarkOrange));
            var vertexPen = new Pen(Color.FromArgb(192, Color.Red), 1);
            var edgePen = new Pen(Color.FromArgb(224, Color.Blue), 1);

            var maxWeight = graph.Weights.Max();
            var maxWidth = (int)(maxWeight / graph.Weights.Min());
            maxWidth = maxWeight > 10 ? 10 : maxWidth;

            var bitmap = new Bitmap(size.Width, size.Height);
            using (var image = Graphics.FromImage(bitmap))
            {
                foreach (var vertex in graph.Vertices)
                {

                    foreach (var second in graph.SymAdj(vertex))
                    {
                        var points = new PointF[3];
                        var a = new PointF((float)((cX[vertex] + cX[second]) / 2), (int)((cY[vertex] + cY[second]) / 2));
                        a.X += 10;
                        a.Y -= 10;

                        points[0] = new PointF((float)cX[vertex], (float)cY[vertex]);
                        points[1] = a;
                        points[2] = new PointF((float)cX[second], (float)cY[second]);
                        edgePen.Width = (int)(graph.Weight(vertex, second) / maxWeight * maxWidth);
                        image.DrawBSpline(edgePen, points, 0.3f, 0.01f);

                        //image.DrawLine(edgePen, (float)cX[vertex], (float)cY[vertex], (float)cX[second], (float)cY[second]);
                    }
                }

                const int scale = 1;
                var fill = graph.Radiuses.Max() < (Math.Max(size.Height, (float) size.Width) / 20);
                for (var i = 0; i < graph.VerticesAmount; i++)
                {
                    var radius = graph.Radius(i)*scale;
                    var x = cX[i];
                    var y = cY[i];

                    x -= radius;
                    y -= radius;

                    var side = (float) radius*2;
                    if (fill)
                    image.FillEllipse(vertexBrush, (float) x, (float) y, side, side);
                    image.DrawEllipse(vertexPen, (float) x, (float) y, side, side);
                }
            }
            var file = string.Format("{0}. {1}", m_number++, fileName);
            if (m_firstFile == null)
                m_firstFile = file;
            bitmap.Save(file);
            edgePen.Dispose();
            vertexPen.Dispose();
        }

        public static void OpenFirst()
        {
            if (File.Exists(m_firstFile))
                Process.Start(m_firstFile);
        }
    }

    internal static class GraphicsExtension
    {
        private static void DrawCubicCurve(this Graphics graphics, Pen pen, float beta, float step, PointF start,
            PointF end, float a3, float a2, float a1, float a0, float b3, float b2, float b1, float b0)
        {
            var stop = false;

            var xPrev = beta*a0 + (1 - beta)*start.X;
            var yPrev = beta*b0 + (1 - beta)*start.Y;

            for (var t = step;; t += step)
            {
                if (stop)
                    break;

                if (t >= 1)
                {
                    stop = true;
                    t = 1;
                }

                var xNext = beta*(a3*t*t*t + a2*t*t + a1*t + a0) + (1 - beta)*(start.X + (end.X - start.X)*t);
                var yNext = beta*(b3*t*t*t + b2*t*t + b1*t + b0) + (1 - beta)*(start.Y + (end.Y - start.Y)*t);

                graphics.DrawLine(pen, xPrev, yPrev, xNext, yNext);

                xPrev = xNext;
                yPrev = yNext;
            }
        }

        public static void DrawBSpline(this Graphics graphics, Pen pen, PointF[] points, float beta, float step)
        {
            if (points == null)
                throw new ArgumentNullException("points", "The point array must not be null.");

            if (beta < 0 || beta > 1)
                throw new ArgumentException("The bundling strength must be >= 0 and <= 1.");

            if (step <= 0 || step > 1)
                throw new ArgumentException("The step must be > 0 and <= 1.");

            if (points.Length <= 1)
                return;

            if (points.Length == 2)
            {
                graphics.DrawLine(pen, points[0], points[1]);
                return;
            }

            float a3, a2, a1, a0, b3, b2, b1, b0;
            var deltaX = (points[points.Length - 1].X - points[0].X)/(points.Length - 1);
            var deltaY = (points[points.Length - 1].Y - points[0].Y)/(points.Length - 1);
            PointF start, end;

            {
                a0 = points[0].X;
                b0 = points[0].Y;

                a1 = points[1].X - points[0].X;
                b1 = points[1].Y - points[0].Y;

                a2 = 0;
                b2 = 0;

                a3 = (points[0].X - 2*points[1].X + points[2].X)/6;
                b3 = (points[0].Y - 2*points[1].Y + points[2].Y)/6;

                start = points[0];
                end = new PointF
                    (
                    points[0].X + deltaX,
                    points[0].Y + deltaY
                    );

                graphics.DrawCubicCurve(pen, beta, step, start, end, a3, a2, a1, a0, b3, b2, b1, b0);
            }

            for (var i = 1; i < points.Length - 2; i++)
            {
                a0 = (points[i - 1].X + 4*points[i].X + points[i + 1].X)/6;
                b0 = (points[i - 1].Y + 4*points[i].Y + points[i + 1].Y)/6;

                a1 = (points[i + 1].X - points[i - 1].X)/2;
                b1 = (points[i + 1].Y - points[i - 1].Y)/2;

                a2 = (points[i - 1].X - 2*points[i].X + points[i + 1].X)/2;
                b2 = (points[i - 1].Y - 2*points[i].Y + points[i + 1].Y)/2;

                a3 = (-points[i - 1].X + 3*points[i].X - 3*points[i + 1].X + points[i + 2].X)/6;
                b3 = (-points[i - 1].Y + 3*points[i].Y - 3*points[i + 1].Y + points[i + 2].Y)/6;

                start = new PointF
                    (
                    points[0].X + deltaX*i,
                    points[0].Y + deltaY*i
                    );

                end = new PointF
                    (
                    points[0].X + deltaX*(i + 1),
                    points[0].Y + deltaY*(i + 1)
                    );

                graphics.DrawCubicCurve(pen, beta, step, start, end, a3, a2, a1, a0, b3, b2, b1, b0);
            }

            {
                a0 = points[points.Length - 1].X;
                b0 = points[points.Length - 1].Y;

                a1 = points[points.Length - 2].X - points[points.Length - 1].X;
                b1 = points[points.Length - 2].Y - points[points.Length - 1].Y;

                a2 = 0;
                b2 = 0;

                a3 = (points[points.Length - 1].X - 2*points[points.Length - 2].X + points[points.Length - 3].X)/6;
                b3 = (points[points.Length - 1].Y - 2*points[points.Length - 2].Y + points[points.Length - 3].Y)/6;

                start = points[points.Length - 1];

                end = new PointF
                    (
                    points[0].X + deltaX*(points.Length - 2),
                    points[0].Y + deltaY*(points.Length - 2)
                    );

                graphics.DrawCubicCurve(pen, beta, step, start, end, a3, a2, a1, a0, b3, b2, b1, b0);
            }
        }
    }
}