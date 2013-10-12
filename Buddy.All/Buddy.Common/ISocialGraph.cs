using System.Collections.Generic;

namespace Buddy.Common
{
    public interface ISocialGraph
    {
        IList<Vertex> Vertices { get; }

        IList<Edge> Edges { get; }
    }
}