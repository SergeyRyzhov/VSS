using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Buddy.Common;
using Buddy.Placer;

namespace Buddy.Console
{
    internal class Program
    {
        private static void Main()
        {
            const string filename = "f";
            var rnd = new Random();
            var parser = new SotialParser();
            ISotialGraph graph = parser.Parse(filename);
            var size = new Size(640, 480);
            var coords = new List<PointF>();
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                float x = (float) rnd.NextDouble()*size.Width;
                float y = (float) rnd.NextDouble()*size.Height;
                coords.Add(new PointF(x, y));
            }

            PrintCoordinates(coords);

            DrawGraph(size, graph, coords, "input.bmp");
            System.Console.WriteLine("Число итераций");
            var s = System.Console.ReadLine();
            s = string.IsNullOrEmpty(s) ? "5" : s;
            int a = int.Parse(s);

            var placer = new ForceDirectedPlacer {Iterations = a};
            IList<PointF> result = placer.PlaceGraph(graph, coords, size);

            PrintCoordinates(result);

            DrawGraph(size, graph, result, "output.bmp");
            Process.Start("input.bmp");
            var printer = new ConsolePrinter(graph);
            printer.Print();
        }

        private static void PrintCoordinates(IEnumerable<PointF> coords)
        {
            foreach (PointF point in coords)
            {
                System.Console.WriteLine(point);
            }
        }

        private static void DrawGraph(Size size, ISotialGraph graph, IList<PointF> coords, string fileName)
        {
            var vertextPen = new Pen(Color.Red, 1);
            var edgePen = new Pen(Color.Blue, 1);

            var bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics image = Graphics.FromImage(bitmap))
            {
                foreach (Edge edge in graph.Edges)
                {
                    PointF a = coords[edge.U.Id];
                    PointF b = coords[edge.V.Id];
                    image.DrawLine(edgePen, a, b);
                }

                const int scale = 1;
                foreach (Vertex vertex in graph.Vertices)
                {
                    PointF x = coords[vertex.Id];
                    float radius = vertex.Radius*scale;
                    x.X -= radius/2;
                    x.Y -= radius/2;
                    image.DrawEllipse(vertextPen, x.X, x.Y, radius, radius);
                }
            }
            bitmap.Save(fileName);
            edgePen.Dispose();
            vertextPen.Dispose();
        }
    }
}