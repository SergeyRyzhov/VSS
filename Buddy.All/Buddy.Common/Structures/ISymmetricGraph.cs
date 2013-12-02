using System.Collections.Generic;

namespace Buddy.Common.Structures
{
    public interface ISymmetricGraph
    {
        double[] Radius { get; }

        double[] Weight { get; }

        int[] ColumnIndex { get; }

        int[] RowIndex { get; }

        int EdgesAmount { get; }

        int VerticesAmount { get; }

        IEnumerable<int> Adj(int vertex); 

        /// <summary>
        /// Обновление внутренних структур
        /// </summary>
        void Update();
    }


    public interface IGraph
    {
        double[] Radius { get; }

        double[] Weight { get; }

        int[] Adjency { get; }

        int[] XAdj { get; }

        int EdgesAmount { get; }

        int VerticesAmount { get; }

        IEnumerable<int> Adj(int vertex);

        IEnumerable<int> SymAdj(int vertex);

        IEnumerable<int> Vertices { get;}
    }

    public class Graph : IGraph
    {
        public Graph(int verticesAmount, int[] xAdj, int[] adjency, double[] radius, double[] weight)
        {
            VerticesAmount = verticesAmount;
            EdgesAmount = xAdj[verticesAmount];
            XAdj = xAdj;
            Adjency = adjency;
            Radius = radius;
            Weight = weight;
        }

        public double[] Radius { get; private set; }
        public double[] Weight { get; private set; }
        public int[] Adjency { get; private set; }
        public int[] XAdj { get; private set; }
        public int EdgesAmount { get; private set; }
        public int VerticesAmount { get; private set; }

        public IEnumerable<int> Adj(int vertex)
        {
            for (var i = XAdj[vertex]; i < XAdj[vertex + 1]; i++)
            {
                yield return Adjency[i];
            }
        }

        public IEnumerable<int> SymAdj(int vertex)
        {
            for (var i = XAdj[vertex]; i < XAdj[vertex + 1]; i++)
            {
                if (Adjency[i] <= vertex)
                {
                    continue;
                }
                
                yield return Adjency[i];
            }
        }

        public IEnumerable<int> Vertices
        {
            get
            {
                for (var i = 0; i < VerticesAmount; i++)
                {
                    yield return i;
                }
            }
        }
    }
}