using System;
using System.Collections.Generic;
using System.Threading;

namespace Buddy.Common.Structures
{
    internal class NeighborTest
    {
        public int N { get; set; }

        private double[,] coord;
        private double[] Radius;
        private int[] Indexes;

        public NeighborTest(int n = 10000)
        {
            this.N = n;
            this.CreateRandomGraph(n);
        }

        private void CreateRandomGraph(int n)
        {
            var randx = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Thread.Sleep(10);
            var randy = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Thread.Sleep(10);
            var randR = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            //List<Coordinate> coord = new List<Coordinate>((int)n);

            // массив с координатами из x и y
            coord = new double[2, n];
            Radius = new double[n];
            Indexes = new int[n];

            for (int i = 0; i < n; i++)
            {
                Indexes[i] = i;
            }

            for (int j = 0; j < n; j++)
            {
                // делаем по координате x до 600
                coord[0, j] = Convert.ToDouble(randx.Next(60000)) / 100;

                // делаем по координате y до 400
                coord[1, j] = Convert.ToDouble(randy.Next(40000)) / 100;
            }

            //complete radius
            for (int k = 0; k < n; k++)
            {
                // радиус в пределах 40
                Radius[k] = Convert.ToDouble(randR.Next(4000)) / 100;
            }
        }

        public int[] getNeighbors(Coordinate x, int index)
        {
            List<int> temp = new List<int>((int)N / 20);

            for (int k = 0; k < N; k++)
            {
                if ((Math.Pow((x.X - coord[0, k]), 2) + Math.Pow((x.Y - coord[1, k]), 2) <= Math.Pow(Radius[index], 2)))
                {
                    temp.Add(k);
                }
            }

            return temp.ToArray();
        }

        public void Print(int[] x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                Console.WriteLine("x[{0}] = ", x[i]);
            }
        }

        // проверяем, равны ли массивы вершин,которые найдены моим методом, и которые есть
        public bool EqualsOfArrays(int[] es, int[] ex)
        {
            Array.Sort(es);
            Array.Sort(ex);

            for (int i = 0; i < es.Length; i++)
            {
                if (es[i] != ex[i])
                    return false;
            }

            return true;
        }
    }
}