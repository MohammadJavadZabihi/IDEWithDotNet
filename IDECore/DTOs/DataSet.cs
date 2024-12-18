using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDECore.DTOs
{
    public class DataSet
    {
        public Dictionary<string, Dictionary<string, double>> SyntaxErrors { get; set; }
        public Dictionary<string, Dictionary<string, double>> MissingSymbols { get; set; }
        //public Dictionary<string, string> IndentationErrors { get; set; }
    }
}
