using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    [Flags]
    public enum CastlingRights
    {
        None = 0b0000,
        WhiteKingSide = 0b0001,
        WhiteQueenSide = 0b0010,
        BlackKingSide = 0b0100,
        BlackQueenSide = 0b1000
    }
}
