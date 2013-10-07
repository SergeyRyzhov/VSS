using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy.Common;

namespace Buddy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = "f";//args[1];

            var parser = new SotialParser();
            var graph = parser.Parse(filename);

            var printer = new ConsolePrinter(graph);
            printer.Print();


        }
    }
}
