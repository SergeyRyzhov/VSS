using System.Collections.Generic;
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
                //if (radiuses[current] < 50)
                    radiuses[current] += m_graph.Radiuses[i];
                //radiuses[current] = 1;
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

            MakeGraph(size, map, out xadj, out adjncy, out weight);

            ComputeRadiuses(size, map, out radiuses);

            //ComputeWeights(adjncy.Count, map, out weight);

            return new Graph(size, xadj, adjncy.ToArray(), radiuses, weight);
        }

        protected virtual void MakeGraph(int size, int[] map, out int[] xadj, out List<int> adjncy, out double[] weights)
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
            var listWeights = new List<double>();

            var mask = new int[size + 1];
            for (var i = 0; i <= size; i++) mask[i] = -1;
            xadj[0] = 0;
            for (var i = 0; i < size; i++)
            {
                var j = fst[i];
                var temp = new List<EdgeInfo>();
                while (j != -1)
                {
                    for (int k = m_graph.XAdj[j]; k < m_graph.XAdj[j + 1]; k++)
                    {
                        var e = m_graph.Adjency[k];
                        if (mask[map[e]] != i && map[e] != i)
                        {
                            temp.Add(new EdgeInfo
                            {
                                Vertex = map[e],
                                Weight = m_graph.Weights[k]
                            });

                            mask[map[e]] = i;
                        }
                        else
                        {
                            if (mask[map[e]] == i)
                            {
                                temp.Find(ei => ei.Vertex == map[e]).Weight += m_graph.Weights[k];
                            }
                        }
                    }
                    j = nxt[j];
                }
                adjncy.AddRange(temp.OrderBy(e => e.Vertex).Select(ei => ei.Vertex));
                listWeights.AddRange(temp.OrderBy(ei=>ei.Vertex).Select(ei=>ei.Weight));

                xadj[i + 1] = adjncy.Count;
            }
            weights = listWeights.ToArray();
        }
    }

    internal class EdgeInfo
    {
        public int Vertex;
        public double Weight;
    }
}