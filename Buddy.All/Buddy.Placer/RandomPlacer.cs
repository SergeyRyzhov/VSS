using System;
using System.IO;

namespace Buddy.Placer
{
    public class RandomPlacer : IPlacer, IPersistable
    {
        public void PlaceGraph(int nodes, double[] radiuses, int[] xAdj, int[] adjency, double[] weights, double width,
            double height, double[] initialX, double[] initialY, out double[] resultX, out double[] resultY)
        {
            resultX = new double[nodes];
            resultY = new double[nodes];
            var rnd = new Random();

            for (var i = 0; i < nodes; i++)
            {
                resultX[i] = rnd.NextDouble() * width;
                resultY[i] = rnd.NextDouble() * height;
            }
        }

        public ISettings Settings { get; set; }

        public void Persist(string fileName, double[] x, double[] y)
        {
            File.Delete(fileName);
            var fs = new FileStream(fileName, FileMode.CreateNew);
            var sw = new StreamWriter(fs);

            if (x.Length != y.Length)
            {
                throw new Exception();
            }
            var n = x.Length;
            sw.WriteLine("{0}", n);

            for (var i = 0; i < n; i++)
            {
                sw.WriteLine("{0} {1}", x[i], y[i]);
            }

            sw.Close();
            fs.Close();
        }

        public void Load(string fileName, out double[] x, out double[] y)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            var sr = new StreamReader(fs);

            var line = sr.ReadLine();

            if (line == null)
            {
                throw  new Exception();
            }

            var nodes = Int32.Parse(line);

            x = new double[nodes];
            y = new double[nodes];

            line = sr.ReadLine();

            for (var i = 0; i < nodes; i++)
            {
                if (line == null)
                {
                    throw new Exception();
                }

                var lineNums = line.Split(new []{' '});

                x[i] = double.Parse(lineNums[0]);
                y[i] = double.Parse(lineNums[1]);

                line = sr.ReadLine();
            }

            sr.Close();
            fs.Close();
        }
    }
}