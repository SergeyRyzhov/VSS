using System.Linq;

namespace Buddy.Common.Structures.Mappers
{
    /// <summary>
    /// Обьединение списка смежности очередной вершины в одну вершину нового графа.
    /// </summary>
    public class AllAdjacencyToVertexMapper : IReductionMapper
    {
        public void ReductionMap(int[] map, IGraph graph)
        {
            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                map[i] = -1;
            }

            var current = 0;

            foreach (var vertex in graph.Vertices)
            {
                var first = vertex;
                if (map[first] == -1)
                {
                    map[first] = current;
                }

                foreach (var label in graph.SymAdj(vertex).OrderBy(e => -e))
                {
                    var second = label;
                    if (map[second] == -1)
                    {
                        map[second] = current;

                        //break;
                    }
                }
                current++;
            }

            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                if (map[i] == -1)
                    map[i] = current++;
            }
        }
    }
}