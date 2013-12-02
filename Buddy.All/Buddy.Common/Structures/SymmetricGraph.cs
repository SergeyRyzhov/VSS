using System.Collections.Generic;

namespace Buddy.Common.Structures
{
    public class SymmetricGraph : ISymmetricGraph
    {
        public double[] Radius { get; private set; }

        public double[] Weight { get; private set; }

        public int[] ColumnIndex { get; private set; }

        public int[] RowIndex { get; private set; }

        public int EdgesAmount { get; private set; }

        public int VerticesAmount { get; private set; }

        public SymmetricGraph(int verticesAmount, int edgesAmount)
        {
            VerticesAmount = verticesAmount;
            EdgesAmount = edgesAmount;

            Radius = new double[verticesAmount];
            Weight = new double[edgesAmount];

            ColumnIndex = new int[edgesAmount];
            RowIndex = new int[verticesAmount + 1];
        }

        public SymmetricGraph(int verticesAmount, int edgesAmount, double[] radius, double[] weight, int[] columnIndex, int[] rowIndex)
        {
            VerticesAmount = verticesAmount;
            EdgesAmount = edgesAmount;
            Radius = radius;
            Weight = weight;
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
        }

        public IEnumerable<int> Adj(int vertex)
        {
            for (int i = 0; i < RowIndex[vertex + 1]; i++)
            {
                yield return ColumnIndex[i];
            }
        }

        public virtual void Update()
        {
        }
    }
}