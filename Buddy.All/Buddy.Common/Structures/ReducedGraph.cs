namespace Buddy.Common.Structures
{
    /// <summary>
    /// Декоратор над графом, для сжатия
    /// </summary>
    public class ReducedGraph : IGraph, IReducible
    {
        private readonly IGraph m_graph;

        public ReducedGraph(IGraph graph)
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
        }

        public IGraph Reduce(/*IGraph graph, */uint[] labels) //если выносить, то меньть интерфейс
        {
            throw new System.NotImplementedException();
        }
    }
}