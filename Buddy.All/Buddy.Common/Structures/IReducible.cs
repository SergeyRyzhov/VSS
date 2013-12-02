namespace Buddy.Common.Structures
{
    public interface IReducible
    {
        /// <summary>
        /// Получение редуцированного графа
        /// </summary>
        ISymmetricGraph Reduce(int[] labels);
    }
}