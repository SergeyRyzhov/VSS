using Buddy.Common.Printers;
using Buddy.Common.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Buddy.Placer
{
    public class MultilevelPlaсer : BasePlacer
    {
        private readonly IPlacer m_localPlacer;

        public MultilevelPlaсer(ISettings settings, IPlacer localPlacer)
            : base(settings)
        {
            m_localPlacer = localPlacer;
        }

        public override IList<Coordinate> PlaceGraph(IGraph graph, IList<Coordinate> coordinate, Size size)
        {
            double width = size.Width;
            double height = size.Height;
            var initialX = coordinate.Select(c => c.X).ToArray();
            var initialY = coordinate.Select(c => c.Y).ToArray();
            double[] resultX;
            double[] resultY;

            PlaceGraph(graph.VerticesAmount, graph.Radius, graph.ColumnIndex, graph.RowIndex, graph.Weight, width,
                height, initialX, initialY, out resultX, out resultY);

            return resultX.Select((t, i) => new Coordinate(t, resultY[i])).ToList();
        }

        public override void PlaceGraph(int nodes, double[] radiuses, int[] columnIndexes, int[] rowIndexes,
            double[] weights, double width,
            double height, double[] initialX, double[] initialY, out double[] resultX, out double[] resultY)
        {
            IGraph graph = new Graph(nodes, rowIndexes[nodes], radiuses, weights, columnIndexes, rowIndexes);

            var dec = new ReducedGraph(graph);
            var labels = GetReduceLabels(nodes, graph).ToArray();

            var rgraph = dec.Reduce(labels.Select(x => x).ToArray());

            double[] localInitialX;
            double[] localInitialY;

            ComputePositions(nodes, initialX, initialY, rgraph.VerticesAmount, labels, out localInitialX, out localInitialY);

            double[] localResultX;
            double[] localResultY;

            m_localPlacer.PlaceGraph(rgraph.VerticesAmount, rgraph.Radius, rgraph.ColumnIndex, rgraph.RowIndex,
                rgraph.Weight, width, height, localInitialX, localInitialY, out localResultX, out localResultY);

            Drawer.DrawGraph(new Size((int)width, (int)height), rgraph,
                localResultX.Select((t, i) => new Coordinate(t, localResultY[i])).ToList(), string.Format("multulevel_down_{0}.bmp", nodes), false);

            if (rgraph.VerticesAmount > 50)
            {
                var res = PlaceGraph(rgraph, localResultX.Select((x, i) => new Coordinate(x, localResultY[i])).ToList(),
                    new Size((int)width, (int)height));

                localResultX = res.Select(c => c.X).ToArray();
                localResultY = res.Select(c => c.Y).ToArray();
            }

            resultX = new double[nodes];
            resultY = new double[nodes];
            var rnd = new Random();
            for (int i = 0; i < nodes; i++)
            {
                var j = labels[i];
                resultX[i] = localResultX[j] + rnd.Next((int)(2 * rgraph.Radius[j])) - rgraph.Radius[j];

                resultY[i] = localResultY[j] + rnd.Next((int)(2 * rgraph.Radius[j])) - rgraph.Radius[j];

                if (resultX[i] < 0)
                    resultX[i] = -resultX[i];

                if (resultY[i] < 0)
                    resultY[i] = -resultY[i];

                resultX[i] = Math.Min(width, resultX[i]);
                resultY[i] = Math.Min(height, resultY[i]);
            }

            if (true)
            {
                var xx = resultX.ToArray();
                var yy = resultY.ToArray();

                m_localPlacer.PlaceGraph(graph.VerticesAmount, graph.Radius, graph.ColumnIndex, graph.RowIndex,
                    graph.Weight, width, height, xx, yy, out xx, out yy);
                
                if (yy.Any(double.IsNaN))
                {
                    throw new Exception();
                }
                
                if (xx.Any(double.IsNaN))
                {
                    throw new Exception();
                }

                Drawer.DrawGraph(new Size((int)width, (int)height), graph,
                    xx.Select((t, i) => new Coordinate(t, yy[i])).ToList(),
                    string.Format("multulevel_up_{0}.bmp", nodes), false);

                resultX = xx.ToArray();
                resultY = yy.ToArray();
            }
        }

        private static void ComputePositions(int nodes, double[] initialX, double[] initialY, int count, int[] labels,
            out double[] localInitialX, out double[] localInitialY)
        {
            localInitialX = new double[count];
            localInitialY = new double[count];

            for (int i = 0; i < nodes; i++)
            {
                var local = labels[i];
                var amount = labels.Where(l => l == local).Count();
                var vertices = new int[amount];
                var index = 0;
                for (int k = 0; k < labels.Length; k++)
                {
                    if (labels[k] == local)
                    {
                        vertices[index++] = i;
                    }
                }
                localInitialX[local] = 0;
                localInitialY[local] = 0;

                foreach (var vertex in vertices)
                {
                    localInitialX[local] += initialX[vertex];
                    localInitialY[local] += initialY[vertex];
                }

                localInitialX[local] /= vertices.Length;
                localInitialY[local] /= vertices.Length;
            }
        }

        private static IEnumerable<int> GetReduceLabels(int nodes, IGraph graph)
        {
            var labels = new int[nodes];

            for (int i = 0; i < nodes; i++)
            {
                labels[i] = -1;
            }

            var current = 0;
            for (int i = 0; i < graph.VerticesAmount; i++)
            {
                for (int j = graph.RowIndex[i]; j < graph.RowIndex[i + 1]; j++)
                {
                    var first = graph.ColumnIndex[j];
                    var second = i;

                    if (labels[first] == -1 && labels[second] == -1)
                    {
                        labels[first] = current;
                        labels[second] = current;
                        current++;
                    }
                }
            }

            for (var i = 0; i < nodes; i++)
            {
                if (labels[i] == -1)
                    labels[i] = current;
            }

            return labels;
        }
    }
}