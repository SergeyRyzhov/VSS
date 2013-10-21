using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy.Common.Structures;


namespace Buddy.Placer
{
    class FirstPlacer: BasePlacer
    {
        public double DopuskX {get; set;}

        public double DopuskY { get; set; }


        public FirstPlacer(double dopuskX, double dopuskY, ISettings settings): base(settings)
        {
            DopuskX = dopuskX;
            DopuskY = DopuskY;
        }

        static IList<Coordinate> SearchPerimeterVertexes(IList<Coordinate> coordinates, System.Drawing.Size size, double dopuskX, double dopuskY)
        {
            var ver = from c in coordinates where (((size.Height - c.Y) < dopuskY) || ((size.Height - c.Y) > size.Height - dopuskY)) && (((size.Width - c.X) < dopuskX) || ((size.Width - c.X) > size.Width - dopuskX)) select c;

            return ver.ToList<Coordinate>();
        }

        public override IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, System.Drawing.Size size)
        {
            return SearchPerimeterVertexes(coordinates, size, 5, 5);
        }
    }
}

