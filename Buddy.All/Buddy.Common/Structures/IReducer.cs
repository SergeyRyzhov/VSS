namespace Buddy.Common.Structures
{
    public interface IReducer
    {
        /// <summary>
        /// Получение редуцированного графа
        /// </summary>
        IGraph Reduce(IGraph parantGraph, int[] labels);
    }
}