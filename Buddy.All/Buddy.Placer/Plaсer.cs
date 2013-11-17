using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Buddy.Common.Structures;

namespace Buddy.Placer
{
    public class Plaсer : BasePlacer
    {
        public Plaсer(ISettings settings) : base(settings)
        {
        }

        public override IList<Coordinate> PlaceGraph(ISocialGraph graph, IList<Coordinate> coordinates, Size size)
        {
            throw new NotImplementedException();
        }

        public override IList<Coordinate> PlaceGraph(IGraph graph, IList<Coordinate> coordinate, Size size)
        {
            double width = size.Width;
            double height = size.Height;
            var initialX = coordinate.Select(c => c.X).ToArray();
            var initialY = coordinate.Select(c => c.Y).ToArray(); 
            double[] resultX; 
            double[] resultY;
            PlaceGraph(graph.VerticesAmount, graph.Radius, graph.ColumnIndex, graph.RowIndex, graph.Weight, width,
                height, initialX, initialY, out resultX, out resultY);

            return resultX.Select((t, i) => new Coordinate(t, resultY[i])).ToList();
        }

        public override void PlaceGraph(int nodes, double[] radiuses, int[] columnIndexes, int[] rowIndexes,
            double[] weights, double width,
            double height, double[] initialX, double[] initialY, out double[] resultX, out double[] resultY)
        {
            var localPlaser = new ForceDirectedCSR(Settings);

            IGraph graph = new Graph(nodes, rowIndexes[nodes - 1], radiuses, weights, columnIndexes, rowIndexes);

            var dec = new ReducedGraph(graph);

            var labels = GetReduceLabels(nodes, graph);

            //IGraph rgraph = dec.Reduce(labels.Select(x => (int)x).ToArray());

            IGraph rgraph = graph;

            localPlaser.PlaceGraph(rgraph.VerticesAmount, rgraph.Radius, rgraph.ColumnIndex, rgraph.RowIndex,
                rgraph.Weight, width, height, initialX, initialY, out resultX, out resultY);


            //localPlaser.PlaceGraph(nodes, radiuses, columnIndexes, rowIndexes, weights, width, height, initialX,initialY, out resultX, out resultY);
        }

        private static IEnumerable<int> GetReduceLabels(int nodes, IGraph graph)
        {
            var labels = new int[nodes];

            for (int i = 0; i < nodes; i++)
            {
                labels[i] = -1;
            }

            var current = 0;
            for (int i = 0; i < graph.VerticesAmount; i++)
            {
                for (int j = graph.RowIndex[i]; j < graph.RowIndex[i + 1]; j++)
                {
                    var first = graph.ColumnIndex[j];
                    var second = i;

                    if (labels[first] == -1 && labels[second] == -1)
                    {
                        labels[first] = current;
                        labels[second] = current;
                        current ++;
                    }
                }
            }

            for (var i = 0; i < nodes; i++)
            {
                if (labels[i] == -1)
                    labels[i] = current;
            }

            return labels;
        }
    }
}