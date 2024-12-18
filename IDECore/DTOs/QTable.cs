using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDECore.DTOs
{
    public class QTable
    {
        public Dictionary<string, Dictionary<string, int>> SyntaxErrors { get; set; }
        public Dictionary<string, Dictionary<string, int>> MissingSymbols { get; set; }
    }   
}
