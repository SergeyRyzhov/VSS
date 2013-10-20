using System.Collections.Generic;
using System.Drawing;
using Buddy.Common.Structures;

namespace Buddy.Placer
{
    public abstract class BasePlacer : IPlacer
    {
        protected BasePlacer(ISettings settings)
        {
            Settings = settings;
        }

        public abstract IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, Size size);

        public void PlaceGraph(int nodes, int[] radiuses, int[] columnIndexes, int[] rowIndexes, int[] weights, double width,
            double height, double[] initialX, double[] initialY, out double[] resultX, out double[] resultY)
        {
            //TODO converter
            throw new System.NotImplementedException();
        }

        public ISettings Settings { get; private set; }
    }
}
