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

        public virtual void PlaceGraph(uint nodes, double[] radiuses, uint[] columnIndexes, uint[] rowIndexes, double[] weights, double width,
            double height, double[] initialX, double[] initialY, out double[] resultX, out double[] resultY)
        {
            IGraph graph = new Graph((uint)nodes, rowIndexes[rowIndexes.Length - 1], radiuses, weights, columnIndexes, rowIndexes);
            var coordinate = new List<Coordinate>();
            for (var i = 0; i < nodes; i++)
            {
                coordinate.Add(new Coordinate(initialX[i], initialY[i]));
            }
            var size = new Size((int)width, (int)height);
            var coord = PlaceGraph(graph, coordinate, size);
            resultX = new double[nodes];
            resultY = new double[nodes];
            for (var i = 0; i < nodes; i++)
            {
                resultX[i] = coord[i].X;
                resultY[i] = coord[i].Y;
            }
        }

       
        public abstract IList<Coordinate> PlaceGraph(IGraph graph, IList<Coordinate> coordinate, Size size);
        
        public ISettings Settings { get; private set; }
    }
}
