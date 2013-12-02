using Buddy.Common.Structures;
using System.Collections.Generic;
using System.Drawing;

namespace Buddy.Common.Printers
{
    public class Drawer
    {
        private static int m_number;

        public static bool Skip { get; set; }

        static Drawer()
        {
            Skip = false;
        }

        public static void DrawGraph(Size size, IGraph graph, IList<Coordinate> coords, string fileName, bool fill)
        {
            if (Skip)
                return;

            var vertexBrush = Brushes.Red;
            var vertexPen = new Pen(Color.Red, 1);
            var edgePen = new Pen(Color.Blue, 1);

            var bitmap = new Bitmap(size.Width, size.Height);
            using (var image = Graphics.FromImage(bitmap))
            {

                foreach (var vertex in graph.Vertices)
                {
                    foreach (var second in graph.SymAdj(vertex))
                    {
                        var a = coords[vertex];
                        var b = coords[second];
                        image.DrawLine(edgePen, a.FloatX, a.FloatY, b.FloatX, b.FloatY);
                    }
                }

                const int scale = 1;
                for (var i = 0; i < graph.VerticesAmount; i++)
                {
                    var x = new Coordinate(coords[i].X, coords[i].Y);
                    var radius = graph.Radius[i] * scale;

                    x.X -= radius / 2;
                    x.Y -= radius / 2;
                    if (fill)
                    {
                        image.FillEllipse(vertexBrush, x.FloatX, x.FloatY, (float)radius, (float)radius);
                    }
                    else
                    {
                        image.DrawEllipse(vertexPen, x.FloatX, x.FloatY, (float)radius, (float)radius);
                    }
                }
            }
            bitmap.Save(string.Format("{0}. {1}", m_number++, fileName));
            edgePen.Dispose();
            vertexPen.Dispose();
        }
    }
}