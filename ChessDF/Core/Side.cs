using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDF.Core
{
    public enum Side
    {
        White,
        Black
    }

    public static class SideExtensions
    {
        public static Side OpposingSide(this Side side) => side switch
        {
            Side.White => Side.Black,
            Side.Black => Side.White,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };
    }
}
