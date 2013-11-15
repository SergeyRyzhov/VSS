namespace Buddy.Common.Structures
{
    public class Graph : IGraph
    {
        public double[] Radius { get; private set; }
        public double[] Weight { get; private set; }

        public uint[] ColumnIndex { get; private set; }
        public uint[] RowIndex { get; private set; }

        public uint EdgesAmount { get; private set; }

        public uint VerticesAmount { get; private set; }

        public Graph(uint verticesAmount, uint edgesAmount)
        {
            VerticesAmount = verticesAmount;
            EdgesAmount = edgesAmount;


            Radius = new double[verticesAmount];
            Weight = new double[edgesAmount];

            ColumnIndex = new uint[edgesAmount];
            RowIndex = new uint[verticesAmount + 1];
        }

        public Graph(uint verticesAmount, uint edgesAmount, double[] radius, double[] weight, uint[] columnIndex, uint[] rowIndex)
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