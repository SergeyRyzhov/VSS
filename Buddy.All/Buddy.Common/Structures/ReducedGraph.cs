using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;

namespace Buddy.Common.Structures
{
    public class ReducedGraph : IGraph, IReducible
    {
        private readonly IGraph m_graph;

        public ReducedGraph(IGraph graph)
        {
            m_graph = graph;
        }

        public double[] Radius { get { return m_graph.Radius; } }
        public double[] Weight { get { return m_graph.Weight; } }
        public uint[] ColumnIndex { get { return m_graph.ColumnIndex; } }
        public uint[] RowIndex { get { return m_graph.RowIndex; } }
        public uint EdgesAmount { get { return m_graph.EdgesAmount; } }
        public uint VerticesAmount { get { return m_graph.VerticesAmount; } }

        public void Update()
        {
            m_graph.Update();
        }

        public IGraph Reduce(uint[] labels)
        {
            var localLabels = labels.Distinct().ToArray();

            var verticesAmount = localLabels.Count();
            var radiuses = new double[verticesAmount];

            var rowIndex = new uint[verticesAmount + 1];

            for (int i = 0; i < verticesAmount; i++)
            {
                var current = labels[i];
                radiuses[current] += Radius[i];
            }


            var mask = new uint[verticesAmount];
            foreach (var label in localLabels)
            {
                var local = label;
                var amount = labels.Where(l => l == local).Count();
                var vertices = new uint[amount];
                var index = 0;
                for (uint i = 0; i < labels.Length; i++)
                {
                    if (labels[i] == label)
                    {
                        vertices[index++] = i;
                    }
                }
                foreach (var u in vertices)
                {
                    for (int i = 0; i < verticesAmount; i++)
                    {
                        var current = labels[i];

                        if (u != i && current != label)
                            mask[label] += 1;
                    }
                }
                
            }

            var edgesAmount = mask.Count(x => x > 0);
            var columnIndex = new uint[edgesAmount];
            var weight = new double[edgesAmount];

            var graph = new Graph((uint)verticesAmount, (uint)edgesAmount, radiuses, weight, columnIndex, rowIndex);

            return graph;
        }

        public void Algorithm()
        {
            //исходная матрица
            int[] rows = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int count = rows.Length;
            //int[] cols = new int[11] { 1, 7, 3, 7, 3, 4, 5, 4, 6, 6, 6 };
            int[] cols = new int[15] { 1, 3, 4, 2, 5, 7, 6, 9, 6, 7, 8, 9, 8, 8, 9 };
            //int[] indexrows = new int[7] { 0, 2, 4, 7, 9, 10, 11 };
            int[] indexrows = new int[10] { 0, 3, 6, 8, 10, 12, 12, 13, 15, 16 };
            //новая матрица
            int[] metka = new int[10] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 };
            int h;
            int maxValue = metka.Max();
            List<List<int>> newVertex = new List<List<int>>();
            for (int i = 0; i < maxValue + 1; i++)
            {
                newVertex.Add(new List<int>());
            }
            for (int i = 0; i < metka.Length; i++)
            {
                newVertex[metka[i]].Add(i);
            }
            List<int> NewIndexCols = new List<int>();// массив столбцов
            List<int> NewNeightVertex = new List<int>(); //список соседних вершин новогой графа
            List<int> newRowIndex = new List<int>();
            newRowIndex.Add(0);
            List<int> allvertex = new List<int>();
            List<int>[] NeightVertexs = new List<int>[count];//соседние вершины для каждой вершины
            int count2 = 0;
            for (int i = 0; i < NeightVertexs.Length; i++)
            {
                NeightVertexs[i] = new List<int>();
            }

            for (int i = 0; i < NeightVertexs.Length; i++)
            {
                int k = indexrows[i];
                if (indexrows[i + 1] == indexrows[indexrows.Length - 1])
                {
                    break;
                }

                for (int ind = indexrows[i]; ind < indexrows[i + 1]; ind++)
                {
                    if (ind == indexrows[i + 1])
                    { break; }
                    int vertex = cols[ind];
                    NeightVertexs[i].Add(vertex);
                    NeightVertexs[vertex].Add(i);
                }
            }

            foreach (List<int> NewV in newVertex) //для каждой метки            
            {

                for (int i = 0; i < NewV.Count; i++)
                {
                    foreach (int vertex in NewV)
                    {
                        foreach (int k in NeightVertexs[vertex])
                        {
                            if (NewNeightVertex.Count == 0)
                            { NewNeightVertex.Add(k); }
                            else
                            {
                                for (int j = 0; j < NewNeightVertex.Count; j++)
                                {

                                    if (k != NewNeightVertex[j] && j == NewNeightVertex.Count - 1)
                                    { NewNeightVertex.Add(k); break; }
                                    if (k == NewNeightVertex[j])
                                    { break; }

                                }
                            }

                        }

                    }
                    foreach (int vertex in NewV)
                    {
                        NewNeightVertex.Remove(vertex);
                    }
                    NewNeightVertex.Sort();

                    //for (int k = 0; k < NewNeightVertex.Count; k++)
                    //{
                    //    int w = -1;
                    //    for (int j = 1; j < NewNeightVertex.Count; j++)
                    //    {
                    //        if (NewNeightVertex[j - 1] == NewNeightVertex[j])
                    //        { w = j; break; }
                    //    }
                    //    if (w == -1)
                    //        break;                        
                    //    NewNeightVertex.Remove(NewNeightVertex[w]);
                    //}

                }
                foreach (int row in NewNeightVertex)
                {
                    int newrow = metka[row];
                    allvertex.Add(newrow);
                }
                NewNeightVertex.Clear();
                allvertex.Sort();
                for (int j = 0; j < allvertex.Count; j++)
                {
                    int w = -1;
                    for (int i = 1; i < allvertex.Count; i++)
                    {
                        if (allvertex[i - 1] == allvertex[i])
                        { w = i; break; }
                    }
                    if (w == -1)
                        break;
                    allvertex.Remove(allvertex[w]);
                }

                foreach (int al in allvertex)
                {
                    if (al > newVertex.IndexOf(NewV))
                    { NewIndexCols.Add(al); }
                }
                if (count2 == NewIndexCols.Count)
                { newRowIndex.Add(count2 + 1); break; }
                int count4 = NewIndexCols.Count - count2;
                count2 = NewIndexCols.Count;
                allvertex.Clear();
                int count3 = newRowIndex[newRowIndex.Count - 1];
                newRowIndex.Add(count4 + count3);

            }
        }
    }
}