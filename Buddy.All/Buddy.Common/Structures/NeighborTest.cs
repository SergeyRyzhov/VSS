using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Buddy.Common.Structures
{
    class NeighborTest
    {
        public uint N { get; set; }
        double[,] coord; 
        double[] Radius; 
        uint[] Indexes;

        public NeighborTest(uint n = 10000)
        {
            this.N = n;
            this.CreateRandomGraph(n);
        }

        void CreateRandomGraph(uint n)
        {
            var randx = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Thread.Sleep(10);
            var randy = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Thread.Sleep(10);
            var randR = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            //List<Coordinate> coord = new List<Coordinate>((int)n);

            // массив с координатами из x и y
            coord = new double[2, n];
            Radius = new double [n];
            Indexes = new uint [n];

            for (uint i = 0; i < n; i++)
            {
                Indexes[i] = i;
            }


            for (uint j = 0; j < n; j++)
            {
                // делаем по координате x до 600
                coord[0, j] = Convert.ToDouble(randx.Next(60000)) / 100;

                // делаем по координате y до 400
                coord[1, j] = Convert.ToDouble(randy.Next(40000)) / 100;
            }

            //complete radius
            for (uint k = 0; k < n; k++)
            {
                // радиус в пределах 40
                Radius[k] = Convert.ToDouble(randR.Next(4000)) / 100;
            }
        }

        public uint[] getNeighbors(Coordinate x, uint index)
        {
            List<uint> temp = new List<uint>((int)N/20);

            for (uint k = 0; k < N; k++)
            {
                if ((Math.Pow((x.X - coord[0, k]), 2) + Math.Pow((x.Y - coord[1, k]), 2) <= Math.Pow(Radius[index], 2)))
                {
                    temp.Add(k);
                }
            }

            return temp.ToArray();
        }

        public void Print (uint [] x)
        {
            for (uint i = 0; i < x.Length; i++)
            {
                Console.WriteLine("x[{0}] = ", x[i]);
            }
        }

        // проверяем, равны ли массивы вершин,которые найдены моим методом, и которые есть
        public bool EqualsOfArrays(uint [] es, uint [] ex)
        {
            Array.Sort(es);
            Array.Sort(ex);

            for (uint i = 0; i < es.Length; i++)
            {
                if (es[i] != ex[i])
                    return false;
            }

            return true;
        }
    }
}
