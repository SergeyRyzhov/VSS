namespace Buddy.Common.Structures
{
    /// <summary>
    /// Декоратор над графом для быстрого получения соседних
    /// </summary>
    public class NeighborGraph : IGraph, INeighbor
    {
        private readonly IGraph m_graph;

        public NeighborGraph(IGraph graph)
        {
            m_graph = graph;
        }

        /// <summary>
        /// Заполнить обращения к внутреннему графу 
        /// </summary>
        public uint[] Indexes { get; private set; }
        public double[] Radius { get; private set; }
        public double[] Weight { get; private set; }
        public uint[] ColumnIndex { get; private set; }
        public uint[] RowIndex { get; private set; }
        public uint EdgesAmount { get; private set; }
        public uint VerticesAmount { get; private set; }

        public void Update()
        {
            m_graph.Update();
            CreateBlocks();
        }

        public uint[] Neighborhood(uint vertex)
        {
            throw new System.NotImplementedException();
        }

        public void CreateBlocks()
        {
            throw new System.NotImplementedException();
        }
    }
}