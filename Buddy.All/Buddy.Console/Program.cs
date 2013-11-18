﻿using Buddy.Common;
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
            //TODO: пока так, потом через аргументы командной строки
            const string filename = "../../../../Matrix/Grids/400.mtx";

            var rnd = new Random();
            var parser = new Parser();
            var graph = parser.ParseCrsGraph(filename);

            var size = new Size(640, 480);
            var coords = new List<Coordinate>();

            for (var i = 0; i < graph.VerticesAmount; i++)
            {
                var x = (float)rnd.NextDouble() * size.Width;
                var y = (float)rnd.NextDouble() * size.Height;
                coords.Add(new Coordinate(x, y));
            }

            Statistic.PrintStatistic(graph, coords.Select(c => c.X).ToArray(), coords.Select(c => c.Y).ToArray());

            //TODO: Печеть информации в параметры
            const bool print = false;

            //TODO: Заполнени окружности вершины в параметры
            const bool fill = false;

            //if (print)
            //    PrintCoordinates(coords);

            Drawer.DrawGraph(size, graph, coords, "zinput.bmp", fill);
            System.Console.WriteLine("Число итераций");
            var s = System.Console.ReadLine();
            s = string.IsNullOrEmpty(s) ? "5" : s;
            var a = int.Parse(s);
            //TODO: число итерайций в параметры

            ISettings settings = new Settings()
            {
                Iterations = 1,
            };

            var localPlacer = new ForceDirectedCSR(new Settings {Iterations = a});

            var placer = new MultilevelPlaсer(settings, localPlacer);

            IList<Coordinate> result = coords.ToList();

            result = placer.PlaceGraph(graph, result, size);

            Statistic.PrintStatistic(graph, result.Select(c => c.X).ToArray(), result.Select(c => c.Y).ToArray());

            Drawer.DrawGraph(size, graph, result, "aoutput.bmp", fill);
            Process.Start("zinput.bmp");
        }
    }
}