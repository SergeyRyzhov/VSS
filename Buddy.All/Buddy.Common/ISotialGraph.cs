using System.Collections.Generic;

namespace Buddy.Common
{
    public interface ISotialGraph
    {
        IList<Vertex> Vertices { get; }

        IList<Edge> Edges { get; }
    }
}