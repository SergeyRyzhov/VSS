using System;

namespace Buddy.Common.Structures
{
    public class GraphGraphBuilderStatistics : IGraphBuilder
    {
        public void GenerateGraph(int verticesAmount, int minDegree, int maxDegree, int width, int height,
            out IGraph graph, out double[] x, out double[] y)
        {
            graph = new Graph(verticesAmount, 1);
            x = new double[1];
            y = new double[1];
        }


        // создаем граф
        public IGraph CreateTestCraph(int verticesamount, short powermin, short powermax) // powermax - powermin - вилка инцидентности
        {
            var rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            //Thread.Sleep(10);
            bool f;

            // делаем массив, где будут храниться количества ненулевых элементов в каждой строке
            int[] cntInRows = new int[verticesamount];

            for (int x = 0; x < verticesamount; x++)
            {
                // устанавливаем число ненулевых элементов в строке, т.е
                // фактически число ребер, которые есть у каждой вершины
                cntInRows[x] = (int)rand.Next(powermin, powermax);
            }

            // создаем экземпляр графа
            int edgesAmount = 0;

            for (int y = 0; y < cntInRows.Length; y++)
            {
                edgesAmount += cntInRows[y];
            }

            var graph = new Graph(verticesamount, edgesAmount);

            // create nodes
            for (int i = 0; i < verticesamount; i++)
            {
                // формируем номера столбцов в строке i
                for (int j = 0; j < cntInRows[i]; j++) // cntInRows[i] т.к для каждой строки свое число ненулевых элементов
                {
                    do
                    {
                        graph.ColumnIndex[i * cntInRows[i] + j] = (int)rand.Next((int)verticesamount);
                        f = false;

                        // заполняем разными числами, поэтому проверка на равенство
                        for (int k = 0; k < j; k++)
                        {
                            if ((graph.ColumnIndex[i * cntInRows[i] + j]) == (graph.ColumnIndex[i * cntInRows[i] + k]))
                                f = true;
                        }
                    }
                    while (f);
                }

                // сортируем номера столбцов в строке i
                for (int t = 0; t < cntInRows[i] - 1; t++)
                {
                    for (int k = 0; k < cntInRows[i] - 1; k++)
                    {
                        if ((graph.ColumnIndex[i * cntInRows[i] + k]) > (graph.ColumnIndex[i * cntInRows[i] + k + 1]))
                        {
                            int tmp = graph.ColumnIndex[i * cntInRows[i] + k];
                            graph.ColumnIndex[i * cntInRows[i] + k] = graph.ColumnIndex[i * cntInRows[i] + k + 1];
                            graph.ColumnIndex[i * cntInRows[i] + k + 1] = tmp;
                        }
                    }
                }
            }

            // Заполняем массив значений
            for (int i = 0; i < edgesAmount; i++)
                graph.Weight[i] = Convert.ToDouble(rand.Next(10000) / 100);

            // Заполняем массив индексов строк
            int c = 0;
            for (int i = 0; i <= verticesamount; i++)
            {
                graph.RowIndex[i] = c;
                c += cntInRows[i];
            }

            return graph;
        }
    }

    public interface IGraphBuilder
    {
        /// <summary>
        /// Генерация графа. Веса и радиусы варшин должны быть сконфигурены.
        /// </summary>
        /// <param name="verticesAmount">Количество вершин</param>
        /// <param name="minDegree">Минимальная степень</param>
        /// <param name="maxDegree">Максимальная степень</param>
        /// <param name="width">Ширина области</param>
        /// <param name="height">Высота области</param>
        /// <param name="graph">Полученный граф</param>
        /// <param name="x">Координаты X начального размещения</param>
        /// <param name="y">Координаты Y начального размещения</param>
        void GenerateGraph(int verticesAmount, int minDegree, int maxDegree, int width, int height,
            out IGraph graph, out double[] x, out double[] y);
    }
}