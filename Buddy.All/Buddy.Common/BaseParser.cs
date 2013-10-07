using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buddy.Common
{
    public abstract class BaseParser : IParser
    {
        public abstract ISotialGraph Parse(string filename);
    }
}
