using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buddy.Common.Structures;
using System;
using System.IO;

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

                nz = int.Parse(values[2])*2;
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

            var graph = new Graph(size, xadj, adjency, radius, weight);
            return graph;
        }

        public ISymmetricGraph ParseSymmetricCrsGraph(string filename)
        {
            var stream = new FileStream(filename, FileMode.Open);
            var reader = new StreamReader(stream);
            char[] delimiters = { ' ' };
            int currentRow = 0;
            int rowPosition = 0;

            var line = reader.ReadLine() ?? "";
            while (line.Contains("%"))
                line = reader.ReadLine() ?? "";

            var matrixInfo = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (matrixInfo.Length != 3)
            {
                throw new Exception("Invalid MTX file");
            }

            var rows = Int32.Parse(matrixInfo[0]);
            var cols = Int32.Parse(matrixInfo[1]);
            var nonzeros = Int32.Parse(matrixInfo[2]);

            var graph = new SymmetricGraph(cols, nonzeros);
            graph.RowIndex[rowPosition++] = 0;
            for (int i = 0; i < nonzeros; i++)
            {
                line = reader.ReadLine();
                var matrixLine = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                if (matrixLine.Length != 3)
                {
                    throw new Exception("Invalid MTX file");
                }

                graph.Weight[i] = Double.Parse(matrixLine[2]);
                var col = Int32.Parse(matrixLine[1]) - 1;
                var row = Int32.Parse(matrixLine[0]) - 1;
                if (row > col) { var tmp = col; col = row; row = tmp; }

                graph.ColumnIndex[i] = col;
                if (currentRow != row)
                {
                    graph.RowIndex[rowPosition++] = i;
                    currentRow = row;
                }

                graph.Radius[row]+= 4;
                graph.Radius[col]+= 4;
            }

            graph.RowIndex[rowPosition++] = nonzeros;
            graph.RowIndex[rowPosition] = nonzeros;
            reader.Close();
            stream.Close();
            return graph;
        }

        public ISocialGraph Parse(string filename)
        {
            var stream = new FileStream(filename, FileMode.Open);
            var reader = new StreamReader(stream);
            char[] delimiters = { ' ' };
            var linecount = 0;
            ISocialGraph graph = new SocialGraph();

            var line = reader.ReadLine();
            while (line != null && line.Contains("%"))
                line = reader.ReadLine();
            for (; ; )
            {
                if (line == null) break;
                if (linecount == 0)
                {
                    var matrixInfo = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    var vertexCount = Int32.Parse(matrixInfo[0]);
                    if (matrixInfo.Length != 3)
                    {
                        throw new Exception("Invalid MTX file");
                    }
                    for (var i = 0; i < vertexCount; i++)
                    {
                        var v = new Vertex { Id = i };
                        graph.Vertices.Add(v);
                    }
                }
                else
                {
                    var rowColValue = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    var uId = Int32.Parse(rowColValue[0]) - 1;
                    var vId = Int32.Parse(rowColValue[1]) - 1;
                    var weight = Double.Parse(rowColValue[2]);

                    var e = new Edge
                    {
                        U = graph.Vertices[uId],
                        V = graph.Vertices[vId],
                        Weight = weight
                    };

                    graph.Vertices[uId].Radius+=4;
                    graph.Vertices[vId].Radius+=4;

                    graph.Edges.Add(e);
                }
                ++linecount;
                line = reader.ReadLine();
            }
            reader.Close();
            stream.Close();
            return graph;
        }
    }
}