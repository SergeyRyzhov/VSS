using System.IO;
using System.Linq;
using Buddy.Common;
using Buddy.Common.Parser;
using Buddy.Common.Printers;
using Buddy.Common.Structures;
using Buddy.Common.Structures.Mappers;
using Buddy.Placer;
using System;
using System.Drawing;
using Buddy.Placer.Placers;

namespace Buddy.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Console.WriteLine(@"Формат 0:[граф] 1:[итерации FD] 2:[многоуровневые итерации] 3:<тип размещения FD/MFD> 4:<тип редукции Adj / First / Half> 5:<загрузить из output.pos true/false> 6:<рисовать промежуточные итерации true/false> 7:<каталог сохранения>"); 
            if (args.Length < 3) 
                return;
            System.Console.Write(@"Текущие ");
            for (var i = 0; i < args.Length; i++)
            {
                System.Console.Write("{0}:{1} ", i, args[i]);
            }
            System.Console.WriteLine();

            var skip = args.Length > 6 && bool.Parse(args[6]);
            var path = args.Length > 7 ? args[7] : string.Empty; 
            path = path.Last() != '/' ? path : path.Substring(0, path.Length - 1);
            Directory.CreateDirectory(path);

            var filename = args[0];

            var parser = new Parser();
            var graph = parser.ParseCrsGraph(filename);

            var size = new Size(1280, 1024);

            var randPlacer = new RandomPlacer();
            double[] x;
            double[] y;

            var saver = (randPlacer as IPersistable);

            var load = args.Length > 5 && bool.Parse(args[5]);

            if (load)
            {
                saver.Load(path + "/output.pos", out x, out y);
            }
            else
            {
                randPlacer.PlaceGraph(graph.VerticesAmount, null, null, null, null, size.Width, size.Height, null, null,
                    out x, out y);
                saver.Persist(path+ "/input.pos", x, y);
            }

            Statistic.PrintStatistic(graph, x, y);
            Drawer.Directory(path);
            Drawer.ScaledDrawGraph(size, graph, x, y, "input.png");
            if (skip)
                Drawer.GlobalPause();

            var a = int.Parse(args[1]);
            var b = int.Parse(args[2]);

            IReductionMapper mapper;
            if (args.Length > 4)
            {
                switch (args[4].ToLowerInvariant())
                {
                    case "adj":
                        mapper = new AllAdjacencyToVertexMapper();
                        break;
                    case "first":
                        mapper = new FirstAdjacencyToVertexMapper();
                        break;
                    default:
                        mapper = new OneEdgeToVertexMapper();
                        break;
                }
            }
            else
            {
                mapper = new OneEdgeToVertexMapper();
            }


            IPlacer fdPlacer = new ForceDirectedCSR(new Settings { Iterations = a });
            IPlacer placer;
            if (args.Length > 3)
            {
                switch (args[3].ToLowerInvariant())
                {
                    case "fd":
                        placer = fdPlacer;
                        break;
                    default:
                        placer = new MultilevelPlaсer(new Settings { Iterations = b }, fdPlacer, mapper);
                        break;
                }
            }
            else
            {
                placer = new MultilevelPlaсer(new Settings { Iterations = b }, fdPlacer, mapper);
            }

            var start = DateTime.Now;
            double[] resultX;
            double[] resultY;
            placer.PlaceGraph(graph.VerticesAmount, graph.Radiuses, graph.XAdj, graph.Adjency, graph.Weights,
                size.Width, size.Height, x, y, out resultX,
                out resultY);
            GraphHelper.Scale(graph, size, ref resultX, ref resultY);
            var workTime = DateTime.Now - start;
            System.Console.WriteLine("Время работы: {0}", workTime);

            Statistic.PrintStatistic(graph, resultX, resultY);
            saver.Persist(path+"/output.pos", resultX, resultY);

            if (skip)
                Drawer.GlobalResume();
            Drawer.ScaledDrawGraph(size, graph, resultX, resultY, "output.png");

            graph.Scale(size, ref resultX, ref resultY, true);
            Drawer.DrawGraph(size, graph, resultX, resultY, "big_output.png");
        }
    }
}