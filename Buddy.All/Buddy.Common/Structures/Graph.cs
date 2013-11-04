namespace Buddy.Common.Structures
{
    public class Graph : IGraph
    {
        public uint[] Indexes { get; private set; }
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

            Indexes = new uint[verticesAmount];

            Radius = new double[verticesAmount];
            Weight = new double[edgesAmount];

            ColumnIndex = new uint[edgesAmount];
            RowIndex = new uint[verticesAmount + 1];
        }

        public virtual void Update()
        {
        }
    }
}