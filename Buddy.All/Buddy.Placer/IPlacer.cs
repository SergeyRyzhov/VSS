namespace Buddy.Placer
{
    public interface IPlacer
    {
        void PlaceGraph(
            int nodes, double[] radiuses,
            int[] columnIndexes, int[] rowIndexes, double[] weights,
            double width, double height,
            double[] initialX, double[] initialY,
            out double[] resultX, out double[] resultY);

        ISettings Settings { get; }
    }
}