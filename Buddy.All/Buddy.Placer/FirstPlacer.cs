using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            DopuskY = dopuskY;
        }

        static IList<Coordinate> SearchPerimeterVertexes(IEnumerable<Coordinate> coordinates, Size size, double dopuskX, double dopuskY)
        {
            var ver = from c in coordinates
                where
                    (((size.Height - c.Y) < dopuskY) || ((size.Height - c.Y) > size.Height - dopuskY)) &&
                    (((size.Width - c.X) < dopuskX) || ((size.Width - c.X) > size.Width - dopuskX))
                select c;

            return ver.ToList<Coordinate>();
        }

        public override IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, Size size)
        {
            return SearchPerimeterVertexes(coordinates, size, 5, 5);
        }

        public override IList<Coordinate> PlaceGraph(IGraph graph, IList<Coordinate> coordinate, Size size)
        {
            throw new NotImplementedException();
        }
    }
}

