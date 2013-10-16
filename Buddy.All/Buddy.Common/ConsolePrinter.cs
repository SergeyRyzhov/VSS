using System;

namespace Buddy.Common
{
    public class ConsolePrinter : IPrinter
    {
        private readonly ISocialGraph m_graph;

        public ConsolePrinter(ISocialGraph graph)
        {
            m_graph = graph;
        }

        public void Vertices()
        {
            for (var i = 0; i < m_graph.Vertices.Count; i++)
            {
                var vertex = m_graph.Vertices[i];
                Console.WriteLine("Vertex:{0} Radius:{1}", vertex.Id, vertex.Radius);
            }
        }

        public void Edges()
        {
            for (var i = 0; i < m_graph.Edges.Count; i++)
            {
                var edge = m_graph.Edges[i];
                Console.WriteLine("Edge:{0} ({1}, {2}) Weight:{3}", edge.Id, edge.U.Id, edge.V.Id, edge.Weight);
            }
        }

        public void Info()
        {
            Console.WriteLine("Sotial graph");
            Console.WriteLine("Vertices amount: {0}", m_graph.Vertices.Count);
            Console.WriteLine("Edges amount: {0}", m_graph.Edges.Count);
        }

        public void Print()
        {
            Info();
            Vertices();
            Edges();
        }
    }
}