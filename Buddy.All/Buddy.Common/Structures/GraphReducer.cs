using System;
using System.Collections.Generic;
using System.Linq;

namespace Buddy.Common.Structures
{
    public class GraphReducer : IReducer
    {
        private readonly IGraph m_graph;

        public GraphReducer(IGraph graph)
        {
            m_graph = graph;
        }

        protected virtual void ComputeRadiuses(int verticesAmount, int[] labels, int newSize, out double[] radiuses)
        {
            radiuses = new double[newSize];
            for (var i = 0; i < m_graph.VerticesAmount; i++)
            {
                var current = labels[i];
                radiuses[current] += m_graph.Radiuses[i];
            }
        }

        protected virtual void ComputeWeights(int verticesAmount, int[] labels, int newSize, out double[] weights)
        {
            weights = new double[newSize];

            for (int i = 0; i < newSize; i++)
            {
                weights[i] = 1;
            }
        }

        public IGraph Reduce(int[] map)
        {
            var size = 0;
            for (var i = 0; i < m_graph.VerticesAmount; i++)
                size = Math.Max(size, map[i]);
            size += 1;
            var fst = new int[size + 1];
            for (var i = 0; i <= size; i++)
                fst[i] = -1;

            var nxt = new int[m_graph.VerticesAmount];
            for (var i = 0; i < m_graph.VerticesAmount; i++)
            {
                nxt[i] = fst[map[i]];
                fst[map[i]] = i;
            }

            var xadj = new int[size + 1];
            var adjncy = new List<int>();

            var mask = new int[size + 1];
            for (var i = 0; i <= size; i++) mask[i] = -1;
            xadj[0] = 0;
            for (var i = 0; i < size; i++)
            {
                var j = fst[i];
                var temp = new List<int>();
                while (j != -1)
                {
                    foreach (var e in m_graph.Adj(j))
                    {
                        if (mask[map[e]] != i && map[e] != i)
                        {
                            temp.Add(map[e]);
                            mask[map[e]] = i;
                        }
                    }
                    j = nxt[j];
                }
                adjncy.AddRange(temp.OrderBy(e => e));
                xadj[i + 1] = adjncy.Count;
            }

            double[] radiuses;
            double[] weight;

            ComputeRadiuses(m_graph.VerticesAmount, map, size, out radiuses);

            ComputeWeights(m_graph.VerticesAmount, map, adjncy.Count, out weight);

            return new Graph(size, xadj, adjncy.ToArray(), radiuses, weight);
        }
    }
}