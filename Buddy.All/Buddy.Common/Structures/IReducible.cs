namespace Buddy.Common.Structures
{
    public interface IReducible
    {
        /// <summary>
        /// Получение редуцированного графа
        /// </summary>
        IGraph Reduce(uint[] labels);
    }
}