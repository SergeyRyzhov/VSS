using Buddy.Common.Printers;
using Buddy.Common.Structures;
using System;
using System.Drawing;
using System.Linq;

namespace Buddy.Placer
{
    public class MultilevelPlaсer : BasePlacer
    {
        private readonly BasePlacer m_localPlacer;

        private readonly Random m_random;

        public MultilevelPlaсer(ISettings settings, IPlacer localPlacer)
            : base(settings)
        {
            m_localPlacer = localPlacer as BasePlacer;
            if (m_localPlacer == null)
                throw new Exception("Bad local placer.");

            m_random = new Random();
        }

        private static void ComputePositions(int size, double[] inX, double[] inY, int[] map, double[] outX, double[] outY)
        {
            for (var i = 0; i < size; i++)
            {
                var local = map[i];
                var vertices = map.Select((v, j) => j).Where(j => map[j] == local);
                var count = map.Count(l => l == local);

                outX[local] = 0;
                outY[local] = 0;

                foreach (var vertex in vertices)
                {
                    outX[local] += inX[vertex];
                    outY[local] += inY[vertex];
                }

                outX[local] /= count;
                outY[local] /= count;
            }
        }

        public override void PlaceGraph(IGraph graph, Size size, double[] inX, double[] inY, ref double[] outX, ref double[] outY)
        {
            Array.Copy(inX, outX, inX.Length);
            Array.Copy(inY, outY, inY.Length);

            var iterations = Settings.Iterations;

            var x = new double[inX.Length];
            var y = new double[inY.Length];

            while (iterations-- > 0)
            {
                Iterations(graph, size, outX, outY, x, y);

                outX = x.ToArray();
                outY = y.ToArray();
            }
        }

        private void Iterations(IGraph graph, Size size, double[] inX, double[] inY, double[] outX, double[] outY)
        {
            var nodes = graph.VerticesAmount;

            var reducer = new GraphReducer(graph);
            int[] map;
            var reducedGraph = reducer.Reduce(out map);
            var count = map.Max() + 1;

            var x = new double[count];
            var y = new double[count];

            ComputePositions(nodes, inX, inY, map, x, y);

            Drawer.DrawGraph(size, reducedGraph, x, y, string.Format("multulevel_down_{0}.bmp", nodes));

            if (nodes > 50)
                Iterations(reducedGraph, size, x, y, x, y);

            m_localPlacer.PlaceGraph(reducedGraph, size, x, y, ref x, ref y);

            RestorePositions(reducedGraph, map, x, y, size, outX, outY);

            Drawer.DrawGraph(size, graph, outX, outY, string.Format("multulevel_up_{0}.bmp", nodes));
        }

        private void RestorePositions(IGraph graph, int[] map, double[] x, double[] y, Size size, double[] outX, double[] outY)
        {
            var nodes = map.Length;
            for (var i = 0; i < nodes; i++)
            {
                var j = map[i];

                outX[i] = x[j] + m_random.Next((int)graph.Radius(j) * 2) - graph.Radius(j);
                outY[i] = y[j] + m_random.Next((int)graph.Radius(j) * 2) - graph.Radius(j);

                outX[i] = Math.Abs(outX[i]);
                var dX = Math.Abs(size.Width - outX[i]);
                outX[i] = size.Width - dX;

                outY[i] = Math.Abs(outY[i]);
                var dY = Math.Abs(size.Height - outY[i]);
                outY[i] = size.Height - dY;
            }
        }
    }
}