using Buddy.Common;
using System.Collections.Generic;
using System.Drawing;
using Buddy.Common.Structures;

namespace Buddy.Placer
{
    public interface IPlacer
    {
        //TODO report with argumnets
        //settings in constructor
        //delete
        IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, Size size);


        void PlaceGraph(int nodes, int[] rowIndexes, int[] colIndexes, int[] weights, int[] coeff, double sizeX,
            double sizeY, double[] x, double[] y, out double[] resultX, out double[] resulY);

        //delete
        int Iterations { get; set; }
    }
}