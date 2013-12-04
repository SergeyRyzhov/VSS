﻿using System;
using System.Collections.Generic;

namespace Buddy.Common.Structures
{
    public class Graph : IGraph
    {
        public Graph(int verticesAmount, int[] xAdj, int[] adjency, double[] radiuses, double[] weights)
        {
            VerticesAmount = verticesAmount;
            EdgesAmount = xAdj[verticesAmount];
            XAdj = xAdj;
            Adjency = adjency;
            Radiuses = radiuses;
            Weights = weights;
        }

        public Graph(int verticesAmount, int edgesAmount)
        {
            VerticesAmount = verticesAmount;
            EdgesAmount = edgesAmount;
            XAdj = new int[verticesAmount+1];
            Adjency = new int[edgesAmount];
            Radiuses = new double[verticesAmount];
            Weights = new double[EdgesAmount];
        }

        public double[] Radiuses { get; private set; }
        public double[] Weights { get; private set; }
        public int[] Adjency { get; private set; }
        public int[] XAdj { get; private set; }
        public int EdgesAmount { get; private set; }
        public int VerticesAmount { get; private set; }

        public double Radius(int vertex)
        {
            return Radiuses[vertex];
        }
        /// <summary>
        /// Slow method, please use weights array if you know index of edge.
        /// </summary>
        /// <param name="u">first vertex</param>
        /// <param name="v">slow vertex</param>
        /// <returns>weight edge or 0</returns>
        [Obsolete("Please use weights arraym if you know index of edge.")]
        public double Weight(int u, int v)
        {
            for (var i = XAdj[u]; i < XAdj[u + 1]; i++)
            {
                if (Adjency[i] == v)
                    return Weights[i];
            }

            return 0;
        }

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