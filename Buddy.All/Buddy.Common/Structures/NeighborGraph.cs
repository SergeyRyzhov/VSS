using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Buddy.Common.Structures
{
    /// <summary>
    /// Декоратор над графом для быстрого получения соседних
    /// </summary>
    public class NeighborGraph : IGraph, INeighbor
    {
        private readonly IGraph m_graph;
        private IGraph graph;

        Node[] ndmass;                             // массив оберток для вершин, где держим все вершины
        Coordinate[,] blockscoordinate;            // массив для хранения координат блоков
        uint m;
        double height;
        double width;

        public NeighborGraph(IGraph graph, uint m)
        {
            m_graph = graph;
            this.m = m;
        }

        /// <summary>
        /// Заполнить обращения к внутреннему графу 
        /// </summary>
        public uint[] Indexes { get; private set; }
        public double[] Radius { get; private set; }
        public double[] Weight { get; private set; }
        public uint[] ColumnIndex { get; private set; }
        public uint[] RowIndex { get; private set; }
        public uint EdgesAmount { get; private set; }
        public uint VerticesAmount { get; private set; }

        public void Update()
        {
            m_graph.Update();
           // CreateBlocks();
        }

        public uint[] Neighborhood(Coordinate x, uint vertex)
        {
            uint tmp = 0;
            double radius = Indexes[vertex];
           
            // ищем номер вершины, т.е. номер блока, куда попадает вершина 
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if ((x.X >= blockscoordinate[i, j].X) && (x.X < blockscoordinate[i, j].X + width))
                    {
                        if ((x.Y >= blockscoordinate[i, j].Y) && (x.Y < blockscoordinate[i, j].Y))
                        {
                            tmp = (uint)(i + j * m);
                        }
                    }
                }
            }
            // используем LINQ, чтобы найти все вершины из соседних блоков

            Array.Sort(ndmass, new SortByNumberOfBlockMin());

            // ищем вершины в блоках над основым
            List<Node> highervertices = (from Node nd in ndmass
                                         where (nd.number <= ((uint)(tmp + 1 - m)) && (nd.number >= ((uint)(tmp + 1 - m)))
                                             )
                                         select nd).ToList<Node>();

            List<Node> vertices = (from Node nd in ndmass
                                   where (nd.number <= ((uint)(tmp + 1)) && (nd.number >= ((uint)(tmp - 1))))
                                   select nd).ToList<Node>();

            // ищем вершины в блоках под основым
            List<Node> lowvertices = (from Node nd in ndmass
                                      where (nd.number <= ((uint)(tmp - 1 + m)) && (nd.number >= ((uint)(tmp + 1 + m))))
                                      select nd).ToList<Node>();

            highervertices.Concat(vertices);
            highervertices.Concat(lowvertices);

            //лист для того, чтобы хранить вершины, которые попадают в радиус вершины данной
            List<Node> nodelist = new List<Node>(highervertices.Count / 2);

            // ищем вершины, которые попадают в радиус данной вершины
            foreach (Node nd in highervertices)
            {
                if ((Math.Pow((x.X - nd.coord.X), 2) + Math.Pow((x.Y - nd.coord.Y), 2) <= Math.Pow(radius, 2)))
                {
                    nodelist.Add(nd);
                }

            }

            // массив, который будет хранить номера возвращаемых вершин, т.е вершин, которые входят в окружность данной вершины.
            uint[] massOfVertices = new uint[nodelist.Count];

            for (int q = 0; q < nodelist.Count; q++)
            {
                massOfVertices[q] = nodelist[q].indexOfVertex;
            }

            return massOfVertices;
        }

        public void CreateBlocks(IList<Coordinate> coordinate, System.Drawing.Size size, ArrayCoordinate arr)
        {
            height = size.Height / m;
            width = size.Width / m;

            ndmass = new Node [coordinate.Count];
            blockscoordinate = new Coordinate[coordinate.Count, coordinate.Count];

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
                            if ((coordinate[t].Y >= leftCorner.Y) && (coordinate[t].Y < leftCorner.Y))
                            {
                                ndmass[indexOfMassive] = new Node(coordinate[t], (uint)(j + i * m), (uint)t);
                                indexOfMassive++;
                            }
                        }
                    }

                }
            }

            Array.Sort(ndmass, new SortByNumberOfBlockMin());
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
    struct Node
    {
        public uint number;
        public Coordinate coord;
        public uint indexOfVertex;

        public Node(Coordinate c, uint number, uint indexOfVertex)
        {
            this.coord = c;
            this.number = number;
            this.indexOfVertex = indexOfVertex;
        }
    }
}


    