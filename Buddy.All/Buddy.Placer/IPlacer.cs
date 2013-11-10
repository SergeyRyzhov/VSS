using System.Collections.Generic;
using System.Drawing;
using Buddy.Common.Structures;

namespace Buddy.Placer
{
    public interface IPlacer
    {
        //TODO report with argumnets, delete this
        IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, Size size);

        /// <summary>
        /// Размещение графа на плоскости заданного размера
        /// </summary>
        /// <param name="nodes"> Количество вершин</param>
        /// <param name="radiuses"> Радиусы вершин</param>
        /// <param name="columnIndexes"> Столбцовые индексы для формата CRS хранения матрицы</param>
        /// <param name="rowIndexes"> Строчные индексы</param>
        /// <param name="weights"> Веса связей между вершинами</param>
        /// <param name="width"> Ширина области</param>
        /// <param name="height"> Высота области</param>
        /// <param name="initialX"> Координаты X начального размещения</param>
        /// <param name="initialY"> Координаты Y начального размещения</param>
        /// <param name="resultX"> Результирующие координаты X</param>
        /// <param name="resultY"> Результирующие координаты Y</param>
        void PlaceGraph(
            int nodes, int[] radiuses,
            int[] columnIndexes, int[] rowIndexes, int[] weights,
            double width, double height,
            double[] initialX, double[] initialY,
            out double[] resultX, out double[] resultY);

        ISettings Settings { get; }
    }
}
