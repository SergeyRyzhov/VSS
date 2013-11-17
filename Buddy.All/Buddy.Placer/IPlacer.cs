using System.Collections.Generic;
using System.Drawing;
using Buddy.Common.Structures;

namespace Buddy.Placer
{
    public interface IPlacer
    {
        //TODO report with argumnets, delete this
        IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, Size size);

       
        void PlaceGraph(
            int nodes, double[] radiuses,
            int[] columnIndexes, int[] rowIndexes, double[] weights,
            double width, double height,
            double[] initialX, double[] initialY,
            out double[] resultX, out double[] resultY);

        ISettings Settings { get; }
    }
}
