using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Core
{
    public record Position(Board Board, Side SideToMove, Square? EnPassantSquare, CastlingRights CastlingRights, int HalfmoveClock)
    {
        public Side OpposingSide => SideToMove switch
        {
            Side.White => Side.Black,
            Side.Black => Side.White,
            _ => throw new IndexOutOfRangeException(nameof(SideToMove))
        };
    }
}
