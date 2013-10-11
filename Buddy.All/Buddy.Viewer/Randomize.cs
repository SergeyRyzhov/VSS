using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleVisualization
{
    class Randomize
    {
        public void CreateRandomCraph(Graph graph, int nodes_anount, int edges_amount, int width, int height)
        {
            Random rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Thread.Sleep(5);
            Random rand1 = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Thread.Sleep(5);
            Random rand2 = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            for (int i = 0; i < nodes_anount; i++)
            {
                graph.AddNode(rand.Next((width - 45)), rand.Next((height - 45)), i, rand1.Next(40));
            }


            try
            {
                for (int j = 0; j < edges_amount; j++)
                {
                    graph.AddEdge(graph.GetNode(rand2.Next(nodes_anount)), graph.GetNode(rand2.Next(nodes_anount)),
                                  rand1.Next());
                }
            }

            catch (NullReferenceException)
            {
                graph.edgeList.Clear();

                for (int j = 0; j < edges_amount; j++)
                {
                    graph.AddEdge(graph.GetNode(rand2.Next(nodes_anount)), graph.GetNode(rand2.Next(nodes_anount)),
                                  rand1.Next());
                }
            }
        }
    }
}
