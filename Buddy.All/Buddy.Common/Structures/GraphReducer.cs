﻿using System.Collections.Generic;
using System.Linq;

namespace Buddy.Common.Structures
{
    public class GraphReducer
    {
        private readonly IGraph m_graph;
        private readonly IReductionMapper m_reductionMapper;

        public GraphReducer(IGraph graph, IReductionMapper reductionMapper)
        {
            m_graph = graph;
            m_reductionMapper = reductionMapper;
        }

        protected virtual void ComputeMap(out int size, out int[] map)
        {
            map = new int[m_graph.VerticesAmount];

            m_reductionMapper.ReductionMap(map, m_graph);

            size = map.Max() + 1;
        }

        protected virtual void ComputeRadiuses(int verticesAmount, int[] labels, out double[] radiuses)
        {
            radiuses = new double[verticesAmount];
            for (var i = 0; i < m_graph.VerticesAmount; i++)
            {
                var current = labels[i];
                radiuses[current] += m_graph.Radiuses[i];
            }
        }

        protected virtual void ComputeWeights(int edgesAmount, int[] labels, out double[] weights)
        {
            weights = new double[edgesAmount];

            for (var i = 0; i < edgesAmount; i++)
            {
                weights[i] = 1;
            }
        }

        public IGraph Reduce(out int[] map)
        {
            int size;
            int[] xadj;
            List<int> adjncy;
            double[] radiuses;
            double[] weight;

            ComputeMap(out size, out map);

            MakeGraph(size, map, out xadj, out adjncy);

            ComputeRadiuses(size, map, out radiuses);

            ComputeWeights(adjncy.Count, map, out weight);

            return new Graph(size, xadj, adjncy.ToArray(), radiuses, weight);
        }

        protected virtual void MakeGraph(int size, int[] map, out int[] xadj, out List<int> adjncy)
        {
            var fst = new int[size + 1];
            for (var i = 0; i <= size; i++)
                fst[i] = -1;

            var nxt = new int[m_graph.VerticesAmount];
            for (var i = 0; i < m_graph.VerticesAmount; i++)
            {
                nxt[i] = fst[map[i]];
                fst[map[i]] = i;
            }

            xadj = new int[size + 1];
            adjncy = new List<int>();

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
        }
    }
}