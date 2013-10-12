using System;

namespace Buddy.Common
{
    public class RandomParser : BaseParser
    {
        public override ISocialGraph Parse(string filename)
        {
            var graph = new SocialGraph();

            var rnd = new Random();
            var n = rnd.Next(3, 10);
            for (int i = 0; i < n; i++)
            {
                graph.AddVertex(new Vertex
                {
                    Id = i,
                    Radius = rnd.Next(150)
                });
            }

            var nz = rnd.Next(15, 25);
            for (var i = 0; i < nz; i++)
            {
                graph.AddEdge(new Edge
                {
                    Id = i,
                    U = graph.Vertices[rnd.Next(graph.Vertices.Count)],
                    V = graph.Vertices[rnd.Next(graph.Vertices.Count)],
                    Weight = rnd.Next(50)
                });
            }

            return graph;
        }
    }
}