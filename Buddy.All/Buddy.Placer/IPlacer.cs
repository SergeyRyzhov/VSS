
namespace Buddy.Placer
{
    public interface IPlacer
    {
        /// <summary>
        /// Размещение вершин графа
        /// </summary>
        /// <param name="nodes">Количество вершин</param>
        /// <param name="radiuses">Радиусы вершин</param>
        /// <param name="xAdj">Границы строк матрицы смежности</param>
        /// <param name="adjency">Ненулевые элементы матрицы смежности</param>
        /// <param name="weights">Веса рёбер</param>
        /// <param name="width">Ширина области размещения</param>
        /// <param name="height">Высота области размещения</param>
        /// <param name="initialX">Координаты x начального размещения</param>
        /// <param name="initialY">Координаты y начального размещения</param>
        /// <param name="resultX">Координаты x полученного размещения</param>
        /// <param name="resultY">Координаты y полученного размещения</param>
        void PlaceGraph(
            int nodes, double[] radiuses,
            int[] xAdj, int[] adjency, double[] weights,
            double width, double height,
            double[] initialX, double[] initialY,
            out double[] resultX, out double[] resultY);

        /// <summary>
        /// Текущие настройки алгоритма размещения
        /// </summary>
        ISettings Settings { get; }
    }
}
