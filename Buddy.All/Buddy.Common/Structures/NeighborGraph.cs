using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using Buddy.Common.Structures;


// отдать  Юле схему построения
namespace NeighborTester
{
    /// <summary>
    /// Декоратор над графом для быстрого получения соседних
    /// </summary>
    public class NeighborGraph : IGraph, INeighbor
    {
       // private readonly IGraph m_graph;
        
        Node[] ndmass;                             // массив оберток для вершин, где держим все вершины
        //double[, ,] blockscoordinate;              // массив для хранения координат блоков: координата X по адресу blockscoordinate[i, j, 0], координата Y по адресу blockscoordinate[i, j, 1]
        int m;                                     // число блоков
        double height;
        double width;


        public NeighborGraph(IGraph graph, System.Drawing.Size size, IList<Coordinate> coordinate)
        {
            this.EdgesAmount = graph.EdgesAmount;
            this.VerticesAmount = graph.VerticesAmount;

            this.Radiuses = graph.Radiuses;
            this.Weights = graph.Weights;

            this.ColumnIndex = graph.Adjency;                 // количество ненулевых элементов в матрице
            this.RowIndex = graph.XAdj;

            // определяем число m
            defineColBlocks(size);

            CreateBlocks(coordinate, size);
        }

        public NeighborGraph(int verticesAmount, int edgesAmount, System.Drawing.Size size, int [] Indexes, double [] radiuses, IList<Coordinate> coordinate)
        {
            this.Indexes = Indexes;
            this.Radiuses = radiuses;

            // определяем число m
            defineColBlocks(size);

            CreateBlocks(coordinate, size);
        }

        /// <summary>
        /// Заполнить обращения к внутреннему графу 
        /// </summary>
        public int[] Indexes { get; private set; }
        public double[] Radiuses { get; private set; }
        public double[] Weights { get; private set; }
        public int[] Adjency { get; private set; }
        public int[] XAdj { get; private set; }
        public int[] ColumnIndex { get; private set; }
        public int[] RowIndex { get; private set; }
        public int EdgesAmount { get; private set; }
        public int VerticesAmount { get; private set; }
        public double Radius(int vertex)
        {
            throw new NotImplementedException();
        }

        public double Weight(int u, int v)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Adj(int vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> SymAdj(int vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Vertices { get; private set; }

        public void Update()
        {
           //m_graph.Update();
           // CreateBlocks();
        }

        // принимаем на вход координаты вершины и индекс данной вершины, vertex - индекс данной вершины
        public IEnumerable<int> Neighborhood(Coordinate x, int vertex)
        {
            // tmp - номер блока куда попадает вершина
            int tmp = 0;
            double radius = Radiuses[vertex];
           
           
            for (int q = 0; q < ndmass.Length; q++ )
            {
                if (ndmass[q].indexOfVertex == vertex)
                {
                    tmp = ndmass[q].number;
                    break;
                }
            }

            // используем LINQ, чтобы найти все вершины из соседних блоков
            IEnumerable<int> vertices = (from Node nd in ndmass
                                   where ((((nd.number <= (tmp + 1)) && (nd.number >= (tmp - 1))) 
                                   || ((nd.number >= (tmp - 1 - m)) && (nd.number <= (tmp + 1 - m))) 
                                   ||  ((nd.number >= (tmp - 1 + m)) && (nd.number <= (tmp + 1 + m)))) && ((Math.Pow((nd.coord.X - x.X), 2) + Math.Pow((nd.coord.Y - x.Y), 2) <= Math.Pow(radius, 2))))
                                   select nd.indexOfVertex);
            return vertices;
        }

        public void CreateBlocks(IList<Coordinate> coordinate, System.Drawing.Size size)
        {
            // определяем ширину и высоту блока
            height = ((double)size.Height) / m;
            width = ((double)size.Width) / m;

            ndmass = new Node [coordinate.Count];
            
            int x = 0;
            int y = 0;

            for (int k = 0; k < coordinate.Count; k++ )
            {
                x = (int)(coordinate[k].X / width);
                y = (int)(coordinate[k].Y / height);

                ndmass[k] = new Node(coordinate[k], (y + x * m), k);
            }


            // проходим по числу блоков, чтобы для каждого блока на лету 
            // узнать, какие именно вершины попадают в каждый конкретный блок
            //int indexOfMassive = 0;
            
            /*
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    // Создаем такую локальную переменную для того, чтобы
                    // заносить туда координаты левого верхнего угла каждого блока
                    // с целью поиска вершин, которые входят в этот блок

                    blockscoordinate[i, j, 0] = width * j;
                    blockscoordinate[i, j, 1] = height * i;

                    for (int t = 0; t < coordinate.Count; t++)
                    {
                        if ((coordinate[t].X >= blockscoordinate[i, j, 0]) && (coordinate[t].X < blockscoordinate[i, j, 0] + width))
                        {
                            if ((coordinate[t].Y >= blockscoordinate[i, j, 1]) && (coordinate[t].Y < blockscoordinate[i, j, 1] + height))
                            {
                                ndmass[indexOfMassive] = new Node(coordinate[t], (j + i * m));
                                indexOfMassive++;
                            }
                        }
                    }

                }
             
            }*/

            Array.Sort(ndmass, new SortByNumberOfBlockMin());
        }

        // определяем m
        private void defineColBlocks (System.Drawing.Size size)
        {
            // определяем m
            double temp = 0, w, h;
            for (uint t = 0; t < Radiuses.Length; t++)
            {
                if (temp < Radiuses[t])
                {
                    temp = Radiuses[t];
                }
            }

            w = size.Width / temp;
            h = size.Height / temp;

            // пусть m = меньшему числу
            m = (int)(w < h ? w : h);
        }

        public int[] Neighborhood(double x, double y, int vertex)
        {
            throw new NotImplementedException();
        }

        public void CreateBlocks(Size size, double[] x, double[] y)
        {
            throw new NotImplementedException();
        }
    }

    public class SortByNumberOfBlockMin : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            if (x.number > y.number)
                return -1;
            if (x.number < y.number)
                return 1;
            return 0;
        }
    }

    // обертка для вершин
    public struct Node
    {
        public Coordinate coord;
        public int number;
        public int indexOfVertex;

        public Node(Coordinate c, int number, int indexOfVertex)
        {
            this.coord = c;
            this.number = number;
            this.indexOfVertex = indexOfVertex;
        }
    }
}


    