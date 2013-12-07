using Buddy.Common.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Buddy.Common.Parser
{
    public class Parser : IParser
    {
        public IGraph ParseCrsGraph(string fileName)
        {
            var fileStream = new FileStream(fileName, FileMode.Open);
            var streamReader = new StreamReader(fileStream);

            var line = streamReader.ReadLine();
            while (line != null && line.Contains("%"))
            {
                line = streamReader.ReadLine();
            }

            var separator = new[] { ' ' };
            if (line == null)
            {
                throw new Exception("Bad file");
            }

            var values = line.Split(separator);
            int size;
            int nz;
            try
            {
                size = int.Parse(values[0]);
                var secondSize = int.Parse(values[1]);

                if (size != secondSize)
                {
                    throw new Exception("Not a square matrix.");
                }

                nz = int.Parse(values[2]) * 2;
            }
            catch (Exception exception)
            {
                throw new Exception("Bad first info line.", exception);
            }
            var crsGraph = new List<int>[size];
            for (var i = 0; i < size; i++)
            {
                crsGraph[i] = new List<int>();
            }

            var dweight = new double[size, size];

            var weight = new double[nz];
            var xadj = new int[size + 1];
            var radius = new double[size];
            var adjency = new int[nz];
            for (var i = 0; i < nz / 2; i++)
            {
                line = streamReader.ReadLine();
                if (line == null)
                {
                    throw new Exception("Missed edge");
                }

                values = line.Split(separator);

                var u = int.Parse(values[0]) - 1;
                var v = int.Parse(values[1]) - 1;
                var w = double.Parse(values[2]);
                crsGraph[u].Add(v);
                crsGraph[v].Add(u);

                radius[u] += 1;
                radius[v] += 1;
                dweight[u, v] = w;
                dweight[v, u] = w;
            }
            xadj[0] = 0;
            for (var i = 0; i < size; i++)
            {
                var adj = crsGraph[i];
                var uAdj = adj.Distinct().ToList();
                for (var j = 0; j < uAdj.Count(); j++)
                {
                    adjency[xadj[i] + j] = uAdj[j];
                    weight[xadj[i] + j] = dweight[i, uAdj[j]];
                }
                xadj[i + 1] = xadj[i] + uAdj.Count();
            }

            streamReader.Close();
            fileStream.Close();

            return new Graph(size, xadj, adjency, radius, weight);
        }
    }
}