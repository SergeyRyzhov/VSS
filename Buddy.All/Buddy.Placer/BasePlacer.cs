using Buddy.Common;
using System.Collections.Generic;
using System.Drawing;
using Buddy.Common.Structures;

namespace Buddy.Placer
{
    public abstract class BasePlacer : IPlacer
    {
        public abstract IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, Size size);

        public void PlaceGraph(int nodes, int[] rowIndexes, int[] colIndexes, int[] weights, int[] coeff, double sizeX, double sizeY,
            double[] x, double[] y, out double[] resultX, out double[] resulY)
        {
            throw new System.NotImplementedException();
        }

        public int Iterations { get; set; }
    }
}