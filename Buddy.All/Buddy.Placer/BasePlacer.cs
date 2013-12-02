using System.Linq;
using Buddy.Common.Structures;
using System.Collections.Generic;
using System.Drawing;

namespace Buddy.Placer
{
    public abstract class BasePlacer : IPlacer
    {
        protected BasePlacer(ISettings settings)
        {
            Settings = settings;
        }

        public virtual void PlaceGraph(int nodes, double[] radiuses, int[] xAdj, int[] adjency, double[] weights, double width,
            double height, double[] initialX, double[] initialY, out double[] resultX, out double[] resultY)
        {
            IGraph graph = new Graph(nodes, xAdj, adjency,radiuses,weights);

            var coordinate = graph.Vertices.Select(vertex => new Coordinate(initialX[vertex], initialY[vertex])).ToList();

            var size = new Size((int)width, (int)height);

            var coord = PlaceGraph(graph, coordinate, size);

            resultX = new double[nodes];
            resultY = new double[nodes];

            foreach (var vertex in graph.Vertices)
            {
                resultX[vertex] = coord[vertex].X;
                resultY[vertex] = coord[vertex].Y;
            }
        }

        public abstract IList<Coordinate> PlaceGraph(IGraph symmetricGraph, IList<Coordinate> coordinate, Size size);

        public ISettings Settings { get; private set; }
    }
}