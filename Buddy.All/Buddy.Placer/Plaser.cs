using System.Collections.Generic;
using System.Drawing;
using Buddy.Common.Structures;

namespace Buddy.Placer
{
    public class Plaser : BasePlacer
    {
        public Plaser(ISettings settings) : base(settings)
        {
        }

        public override IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, Size size)
        {
            throw new System.NotImplementedException();
        }

        public override void PlaceGraph(int nodes, int[] radiuses, int[] columnIndexes, int[] rowIndexes, int[] weights, double width,
            double height, double[] initialX, double[] initialY, out double[] resultX, out double[] resultY)
        {
            var plaser = new ForceDirectedPlacer(Settings);

            //plaser.PlaceGraph();
            resultX = new double[] { 1, 2};
            resultY = new double[] { 1, 2};
            return;
        }
    }
}