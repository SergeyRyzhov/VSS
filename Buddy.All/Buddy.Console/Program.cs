using Buddy.Common;
using Buddy.Common.Parser;
using Buddy.Common.Printers;
using Buddy.Common.Structures;
using Buddy.Common.Structures.Mappers;
using Buddy.Placer;
using System;
using System.Drawing;
using System.Linq;
using Buddy.Placer.Placers;

namespace Buddy.Console
{
    internal class Program
    {
        private static void Main()
        {
            //Drawer.Pause();
            Drawer.Fill = true;

            //TODO: пока так, потом через аргументы командной строки
            const string filename = "../../../../Matrix/grids/100x100.mtx";

            var parser = new Parser();
            var graph = parser.ParseCrsGraph(filename);

            var size = new Size(1280, 1024);

            var randPlacer = new RandomPlacer();
            double[] x;
            double[] y;

            var saver = (randPlacer as IPersistable);

            const bool load = false;

            if (load)
            {
                saver.Load("output.pos", out x, out y);
            }
            else
            {
                randPlacer.PlaceGraph(graph.VerticesAmount, null, null, null, null, size.Width, size.Height, null, null,
                    out x, out y);
                saver.Persist("input.pos", x, y);
            }

            Statistic.PrintStatistic(graph, x, y);

            Drawer.DrawGraph(size, graph, x, y, "input.bmp");

            System.Console.WriteLine("Число итераций");
            var s = System.Console.ReadLine();
            s = string.IsNullOrEmpty(s) ? "5" : s;

            var a = int.Parse(s);
            //TODO: число итерайций в параметры

            s = System.Console.ReadLine();
            s = string.IsNullOrEmpty(s) ? "5" : s;
            var b = int.Parse(s);

            IPlacer localPlacer = new ForceDirectedCSR(new Settings { Iterations = a });

            // ReSharper disable once JoinDeclarationAndInitializer
            IReductionMapper mapper;
            
            //mapper = new AllAdjacencyToVertexMapper();
            //mapper = new FirstAdjacencyToVertexMapper();
            mapper = new OneEdgeToVertexMapper();


            IPlacer placer = new MultilevelPlaсer(new Settings { Iterations = b }, localPlacer, mapper);

            //placer = localPlacer; //расскоментировать чтобы FD

            //Drawer.Pause();
            var start = DateTime.Now;
            double[] resultX;
            double[] resultY;

            placer.PlaceGraph(graph.VerticesAmount, graph.Radiuses, graph.XAdj, graph.Adjency, graph.Weights,
                size.Width, size.Height, x, y, out resultX,
                out resultY);

            var workTime = DateTime.Now - start;
            //Drawer.Resume();
            System.Console.WriteLine("Time: {0}", workTime);

            Statistic.PrintStatistic(graph, resultX, resultY);
            saver.Persist("output.pos", resultX, resultY);

            //растянуть на область чтобы посмотреть;) 
            ForceDirectedCSR.Scale(graph,size,ref resultX,ref resultY, true);

            Drawer.DrawGraph(size, graph, resultX, resultY, "output.bmp");
            Drawer.OpenFirst();
        }
    }
}