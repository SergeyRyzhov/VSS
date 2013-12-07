using System.Collections.Generic;

namespace Buddy.Common.Structures
{
    public interface IGraph
    {
        double[] Radiuses { get; }

        double[] Weights { get; }

        int[] Adjency { get; }

        int[] XAdj { get; }

        int EdgesAmount { get; }

        int VerticesAmount { get; }

        double Radius(int vertex);

        double Weight(int u, int v);

        IEnumerable<int> Adj(int vertex);

        IEnumerable<int> SymAdj(int vertex);

        IEnumerable<int> Vertices { get; }
    }
}