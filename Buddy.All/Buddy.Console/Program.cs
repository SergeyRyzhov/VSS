using Buddy.Common;
using Buddy.Common.Parser;
using Buddy.Common.Printers;
using Buddy.Common.Structures;
using Buddy.Placer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Buddy.Console
{
    internal class Program
    {
        private static void Main()
        {
            Drawer.Skip = false;
            //Drawer.Fill = true;
            
            //TODO: пока так, потом через аргументы командной строки
            const string filename = "../../../../Matrix/grids/400.mtx";

            var parser = new Parser();
            var graph = parser.ParseCrsGraph(filename);

            var size = new Size(640, 480);

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
                randPlacer.PlaceGraph(graph.VerticesAmount,null,null,null,null,size.Width, size.Height,null,null, out x, out y);
                saver.Persist("input.pos",x,y);
            }

            var coords = new List<Coordinate>();

            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                coords.Add(new Coordinate(x[i], y[i]));
            }

            Statistic.PrintStatistic(graph, coords.Select(c => c.X).ToArray(), coords.Select(c => c.Y).ToArray());

            //TODO: Печеть информации в параметры
            const bool print = false;

            //if (print)
            //    PrintCoordinates(coords);

            Drawer.DrawGraph(size, graph, coords, "input.bmp");
            System.Console.WriteLine("Число итераций");
            var s = System.Console.ReadLine();
            s = string.IsNullOrEmpty(s) ? "5" : s;
            var a = int.Parse(s);
            //TODO: число итерайций в параметры

            s = System.Console.ReadLine();
            s = string.IsNullOrEmpty(s) ? "5" : s;
            var b = int.Parse(s);
            ISettings settings = new Settings()
            {
                Iterations = b,
            };

            var localPlacer = new ForceDirectedCSR(new Settings { Iterations = a });

            var placer = new MultilevelPlaсer(settings, localPlacer);

            IList<Coordinate> result = coords.ToList();
            var start = DateTime.Now;
            result = placer.PlaceGraph(graph, result, size);
            var workTime = DateTime.Now - start;
            System.Console.WriteLine("Time: {0}", workTime);

            Statistic.PrintStatistic(graph, result.Select(c => c.X).ToArray(), result.Select(c => c.Y).ToArray());

            saver.Persist("output.pos", result.Select(c => c.X).ToArray(), result.Select(c => c.Y).ToArray());

            //ForceDirectedCSR.Scale(size, result, graph,true);

            Drawer.DrawGraph(size, graph, result, "output.bmp");
            Drawer.OpenFirst();
        }
    }
}