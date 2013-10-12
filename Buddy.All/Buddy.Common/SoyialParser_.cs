using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buddy.Common
{
    class SoyialParser_:BaseParser
    {
        public override ISotialGraph Parse(string filename)
        {
            
            FileStream fstrm = new FileStream(filename, FileMode.Open);
            StreamReader reader = new StreamReader(fstrm);
            string line = reader.ReadLine();
            char[] delimiters = { ' ' };
            int linecount = 0;            
            //int NumRows = 0, NumCows = 0, NumNonzero = 0;
            int VertexCount = 0;
            ISotialGraph graph = new SotialGraph();

            while (line.Contains("%")) line = reader.ReadLine();
            for (; ; )
            {
                
                if (line == null) break;
                if (linecount == 0)
                {
                    string[] matrixInfo = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    VertexCount = Int32.Parse(matrixInfo[0]);
                    if (matrixInfo.Length != 3)
                    {
                        throw new Exception("Invalid MTX file");
                    }
                    for (int i = 0; i < VertexCount; i++)
                    {
                        var v = new Vertex();
                        v.Id = i;
                        graph.Vertices.Add(v);
                    }
                    //NumRows = Int32.Parse(matrixInfo[0]);
                    //NumCows = Int32.Parse(matrixInfo[1]);
                    //NumNonzero = Int32.Parse(matrixInfo[2]);

                }
              
                    else
                    {
                        string[] rowColValue = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                       
                        int UId = Int32.Parse(rowColValue[0]) - 1; // mtx files use one-based indexing.
                        int VId = Int32.Parse(rowColValue[1]) - 1;
                        double weight = Double.Parse(rowColValue[2]);

                        var e = new Edge();
                        e.U = graph.Vertices[UId];
                        e.V = graph.Vertices[VId];
                        e.Weight = weight;

                        graph.Edges.Add(e);

                    }
                    ++linecount;
                    line = reader.ReadLine();                   
                } 
               return graph;
             }
        }
    }

