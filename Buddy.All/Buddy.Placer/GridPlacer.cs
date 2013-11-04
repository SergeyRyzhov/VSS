using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy.Common.Structures;
using System.Drawing;


namespace Buddy.Placer
{
    class GridPlacer//: BasePlacer
    {
        void GridMaker(ISocialGraph graph, IList<Coordinate> coordinates, System.Drawing.Size size, int colCells, double widthCell = 0, double heightCell = 0)
        {
            int height = size.Height / colCells;
            int width = size.Width / colCells;

            List<double> centersOfCells = new List<double>(colCells);

            foreach (double center in centersOfCells)
            {
                
            }
        }

        public /*override*/ IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, System.Drawing.Size size)
        {
            throw new NotImplementedException();
        }
    }
}
