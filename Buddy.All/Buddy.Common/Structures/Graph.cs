namespace Buddy.Common.Structures
{
    public class Graph : IGraph
    {
        public double[] Radius { get; private set; }

        public double[] Weight { get; private set; }

        public int[] ColumnIndex { get; private set; }

        public int[] RowIndex { get; private set; }

        public int EdgesAmount { get; private set; }

        public int VerticesAmount { get; private set; }

        public Graph(int verticesAmount, int edgesAmount)
        {
            VerticesAmount = verticesAmount;
            EdgesAmount = edgesAmount;

            Radius = new double[verticesAmount];
            Weight = new double[edgesAmount];

            ColumnIndex = new int[edgesAmount];
            RowIndex = new int[verticesAmount + 1];
        }

        public Graph(int verticesAmount, int edgesAmount, double[] radius, double[] weight, int[] columnIndex, int[] rowIndex)
        {
            VerticesAmount = verticesAmount;
            EdgesAmount = edgesAmount;
            Radius = radius;
            Weight = weight;
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
        }

        public virtual void Update()
        {
        }
    }
}