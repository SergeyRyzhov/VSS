namespace Buddy.Common.Structures
{
    public interface INeighbor
    {
        /// <summary>
        /// Номера соседних вершин
        /// </summary>
        uint[] Neighborhood(uint vertex);

        /// <summary>
        /// Обновление внутренних структур
        /// </summary>
        void CreateBlocks();
    }
}