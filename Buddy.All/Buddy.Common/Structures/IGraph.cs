namespace Buddy.Common.Structures
{
    public interface IGraph
    {
        double[] Radius { get; }

        double[] Weight { get; }

        int[] ColumnIndex { get; }

        int[] RowIndex { get; }

        int EdgesAmount { get; }

        int VerticesAmount { get; }

        /// <summary>
        /// Обновление внутренних структур
        /// </summary>
        void Update();
    }
}