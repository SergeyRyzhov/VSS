using Buddy.Common;
using Buddy.Common.Parser;
using Buddy.Common.Printers;
using Buddy.Common.Structures;
using Buddy.Placer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Buddy.Console
{
    internal class Program
    {
        private static void Main()
        {
            Drawer.Skip = false;
            
            //TODO: пока так, потом через аргументы командной строки
            const string filename = "../../../../Matrix/grids/400.mtx";

            var rnd = new Random();
            var parser = new Parser();
            var graph = parser.ParseCrsGraph(filename);

            var size = new Size(1280, 960);

            var randPlacer = new RandomPlacer();
            double[] x;
            double[] y;

            var saver = (randPlacer as IPersistable);

            const bool load = true;

            if (load)
            {
                saver.Load("400.pos", out x, out y);
            }
            else
            {
                randPlacer.PlaceGraph(graph.VerticesAmount,null,null,null,null,size.Width, size.Height,null,null, out x, out y);
                saver.Persist("400.pos",x,y);
            }

            var coords = new List<Coordinate>();

            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                coords.Add(new Coordinate(x[i], y[i]));
            }

            Statistic.PrintStatistic(graph, coords.Select(c => c.X).ToArray(), coords.Select(c => c.Y).ToArray());

            //TODO: Печеть информации в параметры
            const bool print = false;

            //TODO: Заполнени окружности вершины в параметры
            const bool fill = false;

            //if (print)
            //    PrintCoordinates(coords);

            Drawer.DrawGraph(size, graph, coords, "input.bmp", fill);
            System.Console.WriteLine("Число итераций");
            var s = System.Console.ReadLine();
            s = string.IsNullOrEmpty(s) ? "5" : s;
            var a = int.Parse(s);
            //TODO: число итерайций в параметры

            ISettings settings = new Settings()
            {
                Iterations = 1,
            };

            var localPlacer = new ForceDirectedCSR(new Settings { Iterations = a });

            var placer = new MultilevelPlaсer(settings, localPlacer);

            IList<Coordinate> result = coords.ToList();
            var start = DateTime.Now;
            result = placer.PlaceGraph(graph, result, size);
            var workTime = DateTime.Now - start;
            System.Console.WriteLine("Time: {0}", workTime);

            Statistic.PrintStatistic(graph, result.Select(c => c.X).ToArray(), result.Select(c => c.Y).ToArray());

            Drawer.DrawGraph(size, graph, result, "output.bmp", fill);
            Process.Start("0. input.bmp");
        }
    }
}