using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buddy.Common
{
    public interface ISotialGraph
    {
        IList<Vertex> Vertices { get; }

         IList<Edge> Edges { get; } 
    }
}
