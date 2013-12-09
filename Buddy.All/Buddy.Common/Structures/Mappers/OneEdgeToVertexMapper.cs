using System.Linq;

namespace Buddy.Common.Structures.Mappers
{
    /// <summary>
    /// Ранее не помеченные смежные вершины обьединяются в одну вершину нового графа.
    /// </summary>
    public class OneEdgeToVertexMapper : IReductionMapper
    {
        public virtual void ReductionMap(int[] map, IGraph graph)
        {
            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                map[i] = -1;
            }

            var current = 0;

            foreach (var vertex in graph.Vertices)
            {
                foreach (var label in graph.SymAdj(vertex).OrderBy(e => -e))
                {
                    var second = label;
                    var first = vertex;
                    if (map[second] == -1 && map[first] == -1)
                    {
                        map[second] = current;
                        map[first] = current;

                        current++;
                        break;
                    }
                }
            }

            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                if (map[i] == -1)
                    map[i] = current++;
            }
        }
    }
}