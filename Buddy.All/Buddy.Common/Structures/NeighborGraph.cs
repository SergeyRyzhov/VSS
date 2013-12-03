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
        Coordinate[,] blockscoordinate;            // массив для хранения координат блоков
        int m;                                     // число блоков
        double height;
        double width;

        public NeighborGraph(IGraph graph, System.Drawing.Size size, IList<Coordinate> coordinate)
        {
            this.EdgesAmount = graph.EdgesAmount;
            this.VerticesAmount = graph.VerticesAmount;

            //this.Indexes = graph.Indexes;

            this.Radius = graph.Radius;
            this.Weight = graph.Weight;

            this.ColumnIndex = graph.Adjency;                 // количество ненулевых элементов в матрице
            this.RowIndex = graph.XAdj;

            // определяем число m
            defineColBlocks(size);

            CreateBlocks(coordinate, size);
        }

        public NeighborGraph(int verticesAmount, int edgesAmount, System.Drawing.Size size, int [] Indexes, double [] Radius, IList<Coordinate> coordinate)
        {
            this.Indexes = Indexes;
            this.Radius = Radius;

            // определяем число m
            defineColBlocks(size);

            CreateBlocks(coordinate, size);
        }

        /// <summary>
        /// Заполнить обращения к внутреннему графу 
        /// </summary>
        public int[] Indexes { get; private set; }
        public double[] Radius { get; private set; }
        public double[] Weight { get; private set; }
        public int[] ColumnIndex { get; private set; }
        public int[] RowIndex { get; private set; }
        public int EdgesAmount { get; private set; }
        public int VerticesAmount { get; private set; }

        public void Update()
        {
           //m_graph.Update();
           // CreateBlocks();
        }

        // принимаем на вход координаты вершины и индекс данной вершины, vertex - индекс данной вершины
        public int[] Neighborhood(Coordinate x, int vertex)
        {
            // tmp - номер блока куда попадает вершина
            int tmp = 0;
            double radius = Radius[vertex];
           
            // ищем номер вершины, т.е. номер блока, куда попадает вершина 
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if ((x.X >= blockscoordinate[i, j].X) && (x.X < blockscoordinate[i, j].X + width))
                    {
                        if ((x.Y >= blockscoordinate[i, j].Y) && (x.Y < blockscoordinate[i, j].Y + height))
                        {
                            tmp = (j + i * m);
                        }
                    }
                }
            }
            // используем LINQ, чтобы найти все вершины из соседних блоков

            // Array.Sort(ndmass, new SortByNumberOfBlockMin());

            // ищем вершины в блоках над основым
            List<Node> highervertices = (from Node nd in ndmass
                                         where (nd.number >= ((int)(tmp - 1 - m)) && (nd.number <= ((int)(tmp + 1 - m)))
                                             )
                                         select nd).ToList<Node>();

            List<Node> vertices = (from Node nd in ndmass
                                   where (nd.number <= ((int)(tmp + 1)) && (nd.number >= ((int)(tmp - 1))))
                                   select nd).ToList<Node>();

            // ищем вершины в блоках под основым
            List<Node> lowvertices = (from Node nd in ndmass
                                      where (nd.number >= ((int)(tmp - 1 + m)) && (nd.number <= ((int)(tmp + 1 + m))))
                                      select nd).ToList<Node>();

            IEnumerable<Node> allneighbourvertices = vertices.Concat(lowvertices.Concat(highervertices));

            //лист для того, чтобы хранить вершины, которые попадают в радиус вершины данной
            List<int> nodelist = new List<int>((highervertices.Count + vertices.Count + lowvertices.Count) / 2);

            // ищем вершины, которые попадают в радиус данной вершины
            foreach (Node nd in allneighbourvertices)
            {
                if ((Math.Pow((nd.coord.X - x.X), 2) + Math.Pow((nd.coord.Y - x.Y), 2) <= Math.Pow(radius, 2)))
                {
                    nodelist.Add(nd.indexOfVertex);
                }

            }

            return nodelist.ToArray();
        }

        public void CreateBlocks(IList<Coordinate> coordinate, System.Drawing.Size size)
        {
            // определяем ширину и высоту блока
            height = ((double)size.Height) / m;
            width = ((double)size.Width) / m;

            ndmass = new Node [coordinate.Count];
            blockscoordinate = new Coordinate[m, m];

            // проходим по числу блоков, чтобы для каждого блока на лету 
            // узнать, какие именно вершины попадают в каждый конкретный блок
            int indexOfMassive = 0;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    // Создаем такую локальную переменную для того, чтобы
                    // заносить туда координаты левого верхнего угла каждого блока
                    // с целью поиска вершин, которые входят в этот блок

                    Coordinate leftCorner = new Coordinate(width * j, height * i);
                    blockscoordinate[i, j] = leftCorner;

                    for (int t = 0; t < coordinate.Count; t++)
                    {
                        if ((coordinate[t].X >= leftCorner.X) && (coordinate[t].X < leftCorner.X + width))
                        {
                            if ((coordinate[t].Y >= leftCorner.Y) && (coordinate[t].Y < leftCorner.Y + height))
                            {
                                ndmass[indexOfMassive] = new Node(coordinate[t], (j + i * m), t);
                                indexOfMassive++;
                            }
                        }
                    }

                }
            }


            Array.Sort(ndmass, new SortByNumberOfBlockMin());
        }

        // определяем m
        private void defineColBlocks (System.Drawing.Size size)
        {
            // определяем m
            double temp = 0, w, h;
            for (int t = 0; t < Radius.Length; t++)
            {
                if (temp < Radius[t])
                {
                    temp = Radius[t];
                }
            }

            w = size.Width / temp;
            h = size.Height / temp;

            // пусть m = меньшему числу
            m = (int)(w < h ? w : h);
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


    