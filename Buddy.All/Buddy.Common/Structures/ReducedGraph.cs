using System;
using System.Linq;

namespace Buddy.Common.Structures
{
    public class ReducedGraph : IGraph, IReducible
    {
        private readonly IGraph m_graph;

        public ReducedGraph(IGraph graph)
        {
            m_graph = graph;
        }

        public double[] Radius { get { return m_graph.Radius; } }

        public double[] Weight { get { return m_graph.Weight; } }

        public int[] ColumnIndex { get { return m_graph.ColumnIndex; } }

        public int[] RowIndex { get { return m_graph.RowIndex; } }

        public int EdgesAmount { get { return m_graph.EdgesAmount; } }

        public int VerticesAmount { get { return m_graph.VerticesAmount; } }

        public void Update()
        {
            m_graph.Update();
        }

        public IGraph Reduce(int[] labels)
        {
            var localLabels = labels.Distinct().ToArray();

            var verticesAmount = localLabels.Count();
            var radiuses = new double[verticesAmount];

            var rowIndex = new int[verticesAmount + 1];

            ComputeRadiuses(labels, radiuses, verticesAmount);

            var edgesAmount = EdgesAmount;
            var columnIndex = new int[edgesAmount];
            var weight = new double[edgesAmount];

            int[] xadjency;
            int[] adjency;
            double[] aweight;

            CrsToGraph(out xadjency, out adjency, out aweight);

            Reduction(labels, localLabels, xadjency, adjency, aweight, columnIndex, rowIndex, weight);

            edgesAmount = rowIndex[verticesAmount];
            columnIndex = SubArray(columnIndex, 0, edgesAmount);
            weight = SubArray(weight, 0, edgesAmount);

            var graph = new Graph(verticesAmount, edgesAmount, radiuses, weight, columnIndex, rowIndex);

            return graph;
        }

        private static T[] SubArray<T>(T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        private static void Reduction(int[] labels, int[] localLabels, int[] xadjency, int[] adjency, double[] aweight, int[] columnIndex,
            int[] rowIndex, double[] weight)
        {
            var lastI = 0;
            var mask = new int[localLabels.Length];
            foreach (var vertex in localLabels)
            {
                var local = vertex;
                var amount = labels.Where(l => l == local).Count();
                var vertices = new int[amount];
                var index = 0;
                for (var i = 0; i < labels.Length; i++)
                {
                    if (labels[i] == vertex)
                    {
                        vertices[index++] = i;
                    }
                }

                foreach (var i in vertices)
                {
                    for (var pos = xadjency[i]; pos < xadjency[i + 1]; pos++)
                    {
                        var second = adjency[pos];
                        var secondLabel = labels[second];
                        if (secondLabel > vertex)
                        {
                            if (mask[secondLabel] == 0)
                            {
                                mask[secondLabel] = 1;
                                weight[lastI] = aweight[secondLabel];
                                columnIndex[lastI++] = secondLabel;
                            }
                        }
                    }
                }

                rowIndex[vertex + 1] = lastI;
                Array.Sort(columnIndex, rowIndex[vertex], lastI - rowIndex[vertex]);
                Array.Clear(mask, 0, localLabels.Length);
            }
        }

        protected virtual void CrsToGraph(out int[] xadjency, out int[] adjency, out double[] aweight)
        {
            var n = VerticesAmount;
            var nz = EdgesAmount * 2;

            xadjency = new int[n + 1];
            adjency = new int[nz];
            aweight = new double[nz];

            for (int i = 0; i < nz; i++)
            {
                adjency[i] = -1;
            }

            var lastI = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    var second = -1;
                    for (int k = RowIndex[j]; k < RowIndex[j + 1]; k++)
                    {
                        if (ColumnIndex[k] != i)
                            continue;
                        second = j;
                    }
                    if (second != -1)
                    {
                        aweight[lastI] = Weight[j];
                        adjency[lastI++] = second;
                    }
                }

                for (int j = RowIndex[i]; j < RowIndex[i + 1]; j++)
                {
                    aweight[lastI] = Weight[j];
                    adjency[lastI++] = ColumnIndex[j];
                }

                xadjency[i + 1] = lastI;
            }
        }

        protected virtual void ComputeRadiuses(int[] labels, double[] radiuses, int verticesAmount)
        {
            for (var i = 0; i < VerticesAmount; i++)
            {
                var current = labels[i];
                radiuses[current] += Radius[i] * Radius[i];
            }

            for (var i = 0; i < verticesAmount; i++)
            {
                radiuses[i] = Math.Sqrt(radiuses[i]);
            }
        }
    }
}