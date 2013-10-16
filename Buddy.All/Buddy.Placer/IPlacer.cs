using Buddy.Common;
using System.Collections.Generic;
using System.Drawing;

namespace Buddy.Placer
{
    public interface IPlacer
    {
        IList<PointF> PlaceGraph(ISocialGraph graph, IList<PointF> coordinates, Size size);

        int Iterations { get; set; }
    }
}