using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Buddy.Common;
using Buddy.Common.Parser;
using Buddy.Common.Printers;
using Buddy.Common.Structures;
using Buddy.Placer;

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

            //TODO: Печеть информации в параметры
            const bool print = false;


            //TODO: Заполнени окружности вершины в параметры
            const bool fill = false;

            //if (print)
            //    PrintCoordinates(coords);

            DrawGraph(size, graph, coords, "input.bmp", fill);
            System.Console.WriteLine("Число итераций");
            var s = System.Console.ReadLine();
            s = string.IsNullOrEmpty(s) ? "5" : s;
            var a = int.Parse(s);
            //TODO: число итерайций в параметры

            ISettings settings = new Settings()
            {
                Iterations = 1,
              };
          //  IPlacer placer = new ForceDirectedPlacer(settings);
            var placer = new ForceDirectedCSR(settings);
           IList<Coordinate> result = coords.ToList();
            
            for (var i = 1; i <= a; i++)
            {

                result = placer.PlaceGraph(graph, result, size); //result.ToArray(), size);
              

                //if (print)
                //    PrintCoordinates(result);

                DrawGraph(size, graph, result, string.Format("iteration {0}.bmp", i), fill);
            }


            //if (print)
            //    PrintCoordinates(result);

            DrawGraph(size, graph, result, "output.bmp", fill);
            Process.Start("input.bmp");

          //  var printer = new ConsolePrinter(graph);
           // printer.Info();
        }

        private static void PrintCoordinates(IEnumerable<Coordinate> coords)
        {
            foreach (var point in coords)
            {
                System.Console.WriteLine(point);
            }
        }

        private static void DrawGraph(Size size, IGraph graph, IList<Coordinate> coords, string fileName, bool fill)
        {
            var vertexBrush = Brushes.Red;
            var vertexPen = new Pen(Color.Red, 1);
            var edgePen = new Pen(Color.Blue, 1);

            var bitmap = new Bitmap(size.Width, size.Height);
            using (var image = Graphics.FromImage(bitmap))
            {
                //foreach (var edge in graph.Edges)
                //{
                //    var a = coords[edge.U.Id];
                //    var b = coords[edge.V.Id];
                //    image.DrawLine(edgePen, a.FloatX, a.FloatY, b.FloatX, b.FloatY);
                //}
                for (var i = 0; i < graph.RowIndex.Length - 1; i++)
                {
                    for (var k = graph.RowIndex[i]; k < graph.RowIndex[i + 1]; k++)
                    {
                        var j = (int)graph.ColumnIndex[k];
                        var a = coords[i];
                        var b = coords[j];
                        image.DrawLine(edgePen, a.FloatX, a.FloatY, b.FloatX, b.FloatY);
                    }
                }

                const int scale = 1;
               // foreach (var vertex in graph.Vertices)
                for(var i=0;i<graph.VerticesAmount;i++)
                {
                    //var x = new Coordinate(coords[vertex.Id].X, coords[vertex.Id].Y);
                    //var radius = vertex.Radius * scale;
                    var x = new Coordinate(coords[i].X, coords[i].Y);
                    var radius = graph.Radius[i] * scale;

                    x.X -= radius / 2;
                    x.Y -= radius / 2;
                    if (fill)
                    {
                        image.FillEllipse(vertexBrush, x.FloatX, x.FloatY, (float)radius, (float)radius);
                    }
                    else
                    {
                        image.DrawEllipse(vertexPen, x.FloatX, x.FloatY, (float)radius, (float)radius);
                    }
                }
            }
            bitmap.Save(fileName);
            edgePen.Dispose();
            vertexPen.Dispose();
        }
    }
}