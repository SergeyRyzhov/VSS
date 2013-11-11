using System;
using Buddy.Common.Structures;

namespace Buddy.Common
{
    public class Statistic
    {
        public double SumDistances(IGraph graph, double[] x, double[] y)
        {
            var distance = 0.0;
            for (uint i = 0; i < graph.VerticesAmount; i++)
            {
                for (uint j = graph.RowIndex[i]; j < graph.RowIndex[i + 1]; j++)
                {
                    var first = graph.ColumnIndex[j];
                    var second = i;

                    distance += Distance(x[first], y[first], x[second], y[second]);
                }
            }

            return distance;
        }

        private static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));
        }
    }
}
