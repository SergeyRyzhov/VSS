using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using Buddy.Common.Structures;

namespace NeighborTester
{
    // Сгенерировать коодинаты!!!!
    class GraphBuilderStatistics : IGraphBuilder
    {
        public SymmetricGraph graph;
        public double[] X { get; private set; }
        public double[] Y { get; private set; }

        // создаем граф
        public void CreateTestCraph(int verticesamount, short powermin, short powermax, Size size)        // powermax - powermin - вилка инцидентности
        {
            var rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            //Thread.Sleep(10);
            bool f;
            int edgesAmount = 0;                                                                   // число ребер, задаем дальше

            // делаем массив, где будут храниться количества ненулевых элементов в каждой строке
            int[] cntInRows = new int[verticesamount];

            for (int p = 0; p < verticesamount; p++)
            {
                // устанавливаем число ненулевых элементов в строке, т.е
                // фактически число ребер, которые есть у каждой вершины
                cntInRows[p] = (int)rand.Next(powermin, powermax);
            }


            // создаем экземпляр графа
            for (int y = 0; y < cntInRows.Length; y++)
            {
                edgesAmount += cntInRows[y];
            }

            graph = new SymmetricGraph(verticesamount, edgesAmount);

            int counter = 0;

            // create nodes
            for (int i = 0; i < verticesamount; i++)
            {
                if (i != 0)
                counter += cntInRows[i - 1];

                // формируем номера столбцов в строке i
                for (int j = 0; j < cntInRows[i]; j++) // cntInRows[i] т.к для каждой строки свое число ненулевых элементов
                {
                    do
                    {
                        //graph.ColumnIndex[i * cntInRows[i] + j] = rand.Next(verticesamount);
                        graph.ColumnIndex[counter + j] = rand.Next(verticesamount);
                        f = false;
                        
                        // заполняем разными числами, поэтому проверка на равенство
                        for (int k = 0; k < j; k++)
                        {
                            /*
                            if ((graph.ColumnIndex[i * cntInRows[i] + j]) == (graph.ColumnIndex[i * cntInRows[i] + k]))
                                f = true;
                             */

                            if ((graph.ColumnIndex[counter + j]) == (graph.ColumnIndex[counter + k]))
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
                        //if ((graph.ColumnIndex[i * cntInRows[i] + k]) > (graph.ColumnIndex[i * cntInRows[i] + k + 1]))
                        if ((graph.ColumnIndex[counter + k]) > (graph.ColumnIndex[counter + k + 1]))
                        {
                            /*
                            int tmp = graph.ColumnIndex[i * cntInRows[i] + k];
                            graph.ColumnIndex[i * cntInRows[i] + k] = graph.ColumnIndex[i * cntInRows[i] + k + 1];
                            graph.ColumnIndex[i * cntInRows[i] + k + 1] = tmp;
                            */
                            int tmp = graph.ColumnIndex[counter + k];
                            graph.ColumnIndex[counter + k] = graph.ColumnIndex[counter + k + 1];
                            graph.ColumnIndex[counter + k + 1] = tmp;

                        }

                    }
                }   
            }

            // Заполняем массив значений 
            for (int i = 0; i < edgesAmount; i++) 
                graph.Weight[i] =  Convert.ToDouble(rand.Next(10000) / 100); 
            
            // Заполняем массив индексов строк 
            int c = 0; 
            for (int i = 0; i <= verticesamount; i++)
            {
                graph.RowIndex[i] = c;

                if (i != verticesamount)
                  c += cntInRows[i]; 
            }

            // Заполняем массивы координат

            var randx = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Thread.Sleep(5);
            var randy = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

             X = new double[verticesamount];
             Y = new double[verticesamount];

            for (int k = 0; k < verticesamount; k++ )
            {
                X[k] = Convert.ToDouble(randx.Next(size.Width * 100)) / 100;
                Y[k] = Convert.ToDouble(randy.Next(size.Height * 100)) / 100;   
            }

            //complete radius
            for (int w = 0; w < verticesamount; w++)
            {
                // радиус в пределах 40
                graph.Radius[w] = Convert.ToDouble(rand.Next(4000)) / 100;
            }
        }

        public void GenerateGraph(int verticesAmount, int minDegree, int maxDegree, int width, int height,
            out ISymmetricGraph symmetricGraph, out double[] x, out double[] y)
        {
            throw new NotImplementedException();
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
        /// <param name="symmetricGraph">Полученный граф</param>
        /// <param name="x">Координаты X начального размещения</param>
        /// <param name="y">Координаты Y начального размещения</param>
        void GenerateGraph(int verticesAmount, int minDegree, int maxDegree, int width, int height,
            out ISymmetricGraph symmetricGraph, out double[] x, out double[] y);
    }
}