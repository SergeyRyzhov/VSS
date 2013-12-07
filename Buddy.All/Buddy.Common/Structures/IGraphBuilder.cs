namespace Buddy.Common.Structures
{
    public interface IGraphBuilder
    {
        /// <summary>
        ///     Генерация графа. Веса и радиусы вершин должны быть сконфигурированы.
        /// </summary>
        /// <param name="verticesAmount">Количество вершин</param>
        /// <param name="minDegree">Минимальная степень</param>
        /// <param name="maxDegree">Максимальная степень</param>
        /// <param name="width">Ширина области</param>
        /// <param name="height">Высота области</param>
        ///
        /// <param name="graph">Полученный граф</param>
        /// <param name="x">Координаты X начального размещения</param>
        /// <param name="y">Координаты Y начального размещения</param>
        void GenerateGraph(int verticesAmount, int minDegree, int maxDegree, int width, int height,
            out IGraph graph, out double[] x, out double[] y);
    }
}