using System.Linq;
using Buddy.Common.Structures;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Buddy.Common.Printers
{
    public class Drawer
    {
        private static int m_number;

        private static string m_firstFile;

        private static bool m_skip;

        public static bool Fill { get; set; }

        public static void Pause()
        {
            m_skip = true;
        }

        public static void Resume()
        {
            m_skip = false;
        }

        static Drawer()
        {
            m_skip = false;
            Fill = false;
        }

        public static void DrawGraph(Size size, IGraph graph, double[] cX, double[] cY, string fileName)
        {
            if (m_skip)
                return;

            var vertexBrush = Brushes.OrangeRed;
            var vertexPen = new Pen(Color.DarkOrange, 1);
            var edgePen = new Pen(Color.Blue, 1);

            var bitmap = new Bitmap(size.Width, size.Height);
            using (var image = Graphics.FromImage(bitmap))
            {
                foreach (var vertex in graph.Vertices)
                {
                    foreach (var second in graph.SymAdj(vertex))
                    {
                        image.DrawLine(edgePen, (float)cX[vertex], (float)cY[vertex], (float)cX[second], (float)cY[second]);
                    }
                }

                const int scale = 1;
                for (var i = 0; i < graph.VerticesAmount; i++)
                {
                    var radius = graph.Radius(i) * scale;
                    var x = cX[i];
                    var y = cY[i];

                    x -= radius;
                    y -= radius;

                    var side = (float)radius * 2;

                    if (Fill)
                    {
                        image.FillEllipse(vertexBrush, (float)x, (float)y, side, side);
                    }
                    else
                    {
                        image.DrawEllipse(vertexPen, (float)x, (float)y, side, side);
                    }
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
}