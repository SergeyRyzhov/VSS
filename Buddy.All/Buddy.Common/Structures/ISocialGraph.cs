using System.Collections.Generic;

namespace Buddy.Common.Structures
{
    public interface ISocialGraph
    {
        IList<Vertex> Vertices { get; }

        IList<Edge> Edges { get; }
    }
}