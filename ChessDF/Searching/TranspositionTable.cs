using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Searching
{
    public class TranspositionTable
    {
        private readonly Dictionary<ulong, Node> _nodeDictionary = new();
    }
}
