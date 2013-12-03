using System;
using System.Collections.Generic;
using System.Linq;

namespace Buddy.Common.Structures
{
    public class ReducedGraph : IGraph, IReducible
    {
        private readonly IGraph m_symmetricGraph;

        public ReducedGraph(IGraph symmetricGraph)
        {
            m_symmetricGraph = symmetricGraph;
        }

        protected virtual void ComputeRadiuses(int verticesAmount, int[] labels, int newSize, out double[] radiuses)
        {
            radiuses = new double[newSize];
            for (var i = 0; i < VerticesAmount; i++)
            {
                var current = labels[i];
                radiuses[current] += Radius[i];
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

        public double[] Radius { get { return m_symmetricGraph.Radius; } }

        public double[] Weight { get { return m_symmetricGraph.Weight; } }

        public int[] Adjency { get { return m_symmetricGraph.Adjency; } }

        public int[] XAdj { get { return m_symmetricGraph.XAdj; } }

        public int EdgesAmount { get { return m_symmetricGraph.EdgesAmount; } }

        public int VerticesAmount { get { return m_symmetricGraph.VerticesAmount; } }

        public IEnumerable<int> Adj(int vertex)
        {
            return m_symmetricGraph.Adj(vertex);
        }

        public IEnumerable<int> SymAdj(int vertex)
        {
            return m_symmetricGraph.SymAdj(vertex);
        }

        public IEnumerable<int> Vertices { get; private set; }

        public IGraph Reduce(int[] map)
        {
            var size = 0;
            for (var i = 0; i < VerticesAmount; i++)
                size = Math.Max(size, map[i]);
            size += 1;
            var fst = new int[size + 1];
            for (var i = 0; i <= size; i++)
                fst[i] = -1;

            var nxt = new int[VerticesAmount];
            for (var i = 0; i < VerticesAmount; i++)
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
                    foreach (var e in Adj(j))
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

            ComputeRadiuses(VerticesAmount, map, size, out radiuses);

            ComputeWeights(VerticesAmount, map, adjncy.Count, out weight);

            return new Graph(size, xadj, adjncy.ToArray(), radiuses, weight);
        }
    }
}