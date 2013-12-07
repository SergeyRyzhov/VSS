using Buddy.Common.Structures;
using System.Drawing;

namespace Buddy.Placer
{
    public abstract class BasePlacer : IPlacer
    {
        protected BasePlacer(ISettings settings)
        {
            Settings = settings;
        }

        public virtual void PlaceGraph(int nodes, double[] radiuses, int[] xAdj, int[] adjency, double[] weights,
            double width,
            double height, double[] initialX, double[] initialY, out double[] resultX, out double[] resultY)
        {
            IGraph graph = new Graph(nodes, xAdj, adjency, radiuses, weights);

            var size = new Size((int)width, (int)height);

            resultX = new double[nodes];
            resultY = new double[nodes];

            PlaceGraph(graph, size, initialX, initialY, ref resultX, ref resultY);
        }

        public abstract void PlaceGraph(IGraph graph, Size size, double[] inX, double[] inY, ref double[] outX,
            ref double[] outY);

        public ISettings Settings { get; private set; }
    }
}