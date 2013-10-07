namespace Buddy.Common
{
    public class SotialParser : BaseParser
    {
        public override ISotialGraph Parse(string filename)
        {
            var graph = new SotialGraph();

            var u = new Vertex()
            {
                Radius = 1,
                Id = 1

            };

            var v = new Vertex()
            {
                Radius = 1,
                Id=2
            };

            var e = new Edge()
            {
                U = u,
                V = v,
                Weight = 2,
                Id= 1
            };

            graph.AddVertex(u);
            graph.AddVertex(v);

            graph.AddEdge(e);

            return graph;
        }
    }
}