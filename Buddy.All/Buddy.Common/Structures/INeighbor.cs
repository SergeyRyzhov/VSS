using System.Collections.Generic;
using System.Drawing;

namespace Buddy.Common.Structures
{
    public interface INeighbor
    {
        /// <summary>
        /// Номера соседних вершин
        /// </summary>
        int[] Neighborhood(Coordinate x, int vertex);

        /// <summary>
        /// Обновление внутренних структур
        /// </summary>
        void CreateBlocks(IList<Coordinate> coordinate, Size size, ArrayCoordinate arr);
    }
}