using System;
using System.Drawing;

namespace Buddy.Common.Structures
{
    public static class GraphHelper
    {
        public static void Scale(this IGraph graph, Size size, ref double[] x, ref double[] y, bool always = false)
        {
            var maxX = x[0];
            var minX = x[0];

            var maxY = y[0];
            var minY = y[0];

            var area = 0.0;
            //todo вынести если нужно в настройки
            var limit = 20;
            var limitBorder = 10;

            foreach (var vertex in graph.Vertices)
            {
                maxX = Math.Max(maxX, x[vertex]);
                minX = Math.Min(minX, x[vertex]);

                maxY = Math.Max(maxY, y[vertex]);
                minY = Math.Min(minY, y[vertex]);

                area += Math.PI * graph.Radius(vertex) * graph.Radius(vertex);
            }

            if (area <= size.Height * size.Width)
            {
                limit = 0;
                limitBorder = 0;

                foreach (var vertex in graph.Vertices)
                {
                    maxX = Math.Max(maxX, x[vertex] + graph.Radius(vertex));
                    minX = Math.Min(minX, x[vertex] - graph.Radius(vertex));

                    maxY = Math.Max(maxY, y[vertex] + graph.Radius(vertex));
                    minY = Math.Min(minY, y[vertex] - graph.Radius(vertex));
                }
            }

            if ((maxX > size.Width) || (maxY > size.Height) || (minX < 0) || (minY < 0) || always)
            {
                var weight = maxX - minX;
                var height = maxY - minY;

                var coeffX = (size.Width - limit) / weight;
                var coeffY = (size.Height - limit) / height;

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
    }
}