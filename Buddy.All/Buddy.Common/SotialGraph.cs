using System.Collections.Generic;

namespace Buddy.Common
{
    public class SotialGraph :ISotialGraph
    {
        public SotialGraph()
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