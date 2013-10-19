namespace Buddy.Common.Structures
{
    public interface IGraph
    {
        double[] Radius { get; }
        double[] Weight { get; }

        uint[] ColumnIndex { get; }
        uint[] RowIndex { get; }

        uint EdgesAmount { get; }

        uint VerticesAmount { get; }
    }
}