﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Buddy.Common.Structures
{
    /// <summary>
    /// Декоратор над графом для быстрого получения соседних
    /// </summary>
    public class NeighborSymmetricGraph : ISymmetricGraph, INeighbor
    {
        private readonly ISymmetricGraph m_symmetricGraph;
        private ISymmetricGraph symmetricGraph;

        private Node[] ndmass;                             // массив оберток для вершин, где держим все вершины
        private Coordinate[,] blockscoordinate;            // массив для хранения координат блоков
        private int m;
        private double height;
        private double width;

        public NeighborSymmetricGraph(ISymmetricGraph symmetricGraph, int m)
        {
            m_symmetricGraph = symmetricGraph;
            this.m = m;
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

        public IEnumerable<int> Adj(int vertex)
        {
            return m_symmetricGraph.Adj(vertex);
        }

        public void Update()
        {
            m_symmetricGraph.Update();
            // CreateBlocks();
        }

        public int[] Neighborhood(Coordinate x, int vertex)
        {
            int tmp = 0;
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
                            tmp = (int)(i + j * m);
                        }
                    }
                }
            }
            // используем LINQ, чтобы найти все вершины из соседних блоков

            Array.Sort(ndmass, new SortByNumberOfBlockMin());

            // ищем вершины в блоках над основым
            List<Node> highervertices = (from Node nd in ndmass
                                         where (nd.number <= ((int)(tmp + 1 - m)) && (nd.number >= ((int)(tmp + 1 - m)))
                                             )
                                         select nd).ToList<Node>();

            List<Node> vertices = (from Node nd in ndmass
                                   where (nd.number <= ((int)(tmp + 1)) && (nd.number >= ((int)(tmp - 1))))
                                   select nd).ToList<Node>();

            // ищем вершины в блоках под основым
            List<Node> lowvertices = (from Node nd in ndmass
                                      where (nd.number <= ((int)(tmp - 1 + m)) && (nd.number >= ((int)(tmp + 1 + m))))
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
            int[] massOfVertices = new int[nodelist.Count];

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

            ndmass = new Node[coordinate.Count];
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
                                ndmass[indexOfMassive] = new Node(coordinate[t], (int)(j + i * m), (int)t);
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
    public struct Node
    {
        public int number;
        public Coordinate coord;
        public int indexOfVertex;

        public Node(Coordinate c, int number, int indexOfVertex)
        {
            this.coord = c;
            this.number = number;
            this.indexOfVertex = indexOfVertex;
        }
    }
}