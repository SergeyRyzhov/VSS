using System;
using System.IO;

namespace Buddy.Common
{
    public class SocialParser : BaseParser
    {
        public override ISocialGraph Parse(string filename)
        {
            var stream = new FileStream(filename, FileMode.Open);
            var reader = new StreamReader(stream);
            char[] delimiters = {' '};
            var linecount = 0;
            ISocialGraph graph = new SocialGraph();

            var line = reader.ReadLine();
            while (line != null && line.Contains("%")) 
                line = reader.ReadLine();
            for (;;)
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
                        var v = new Vertex {Id = i};
                        graph.Vertices.Add(v);
                    }
                    //NumRows = Int32.Parse(matrixInfo[0]);
                    //NumCows = Int32.Parse(matrixInfo[1]);
                    //NumNonzero = Int32.Parse(matrixInfo[2]);
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

                    graph.Vertices[uId].Radius++;
                    graph.Vertices[vId].Radius++;

                    graph.Edges.Add(e);
                }
                ++linecount;
                line = reader.ReadLine();
            }
            return graph;
        }
    }
}