using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Buddy.Common.Structures
{
    class GraphBuilderStatistics
    {
        public Graph graph;


        // создаем граф
        void CreateTestCraph(int verticesamount, short powermin, short powermax) // powermax - powermin - вилка инцидентности
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

            graph = new Graph(verticesamount, edgesAmount);


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
                graph.Weight[i] =  Convert.ToDouble(rand.Next(10000) / 100); 
            
            // Заполняем массив индексов строк 
            int c = 0; 
            for (int i = 0; i <= verticesamount; i++)
            {
                graph.RowIndex[i] = c; 
                c += cntInRows[i]; 
            }
        }
    }
}
