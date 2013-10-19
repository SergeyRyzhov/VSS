using System.Collections.Generic;

namespace Buddy.Common.Structures
{
    public class SocialGraph : ISocialGraph
    {
        public SocialGraph()
        {
            Vertices = new List<Vertex>();
            Edges = new List<Edge>();
        }

        public IList<Vertex> Vertices { get; private set; }

        public IList<Edge> Edges { get; private set; }

        public void AddVertex(Vertex vertex)
        {
            Vertices.Add(vertex);
        }

        public void AddEdge(Edge edge)
        {
            Edges.Add(edge);
        }
    }
}