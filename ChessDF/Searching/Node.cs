using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Searching
{
    public record Node(int Depth, double Score, NodeType NodeType);

    public enum NodeType
    {
        Exact,
        UpperBound,
        LowerBound
    }
}
