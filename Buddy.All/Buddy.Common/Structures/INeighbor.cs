using System.Collections.Generic;
using System.Drawing;

namespace Buddy.Common.Structures
{
    public interface INeighbor
    {
        /// <summary>
        /// Номера соседних вершин
        /// </summary>
        int[] Neighborhood(double x, double y, int vertex);

        /// <summary>
        /// Обновление внутренних структур
        /// </summary>
        void CreateBlocks(Size size, double[] x, double[] y);
    }
}