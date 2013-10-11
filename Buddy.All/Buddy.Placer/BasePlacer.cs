using Buddy.Common;
using System.Collections.Generic;
using System.Drawing;

namespace Buddy.Placer
{
    public abstract class BasePlacer : IPlacer
    {
        public abstract IList<PointF> PlaceGraph(ISotialGraph graph, IList<PointF> coordinates, Size size);

        public int Iterations { get; set; }
    }
}