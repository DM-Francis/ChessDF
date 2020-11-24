using ChessDF.Core;
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
        BlackQueenSide = 0b1000,
        All = 0b1111,
        WhiteBoth = 0b0011,
        BlackBoth = 0b1100
    }

    public static class CastlingRightsExtensions
    {
        public static CastlingRights RemoveBoth(this CastlingRights castlingRights, Side side)
        {
            castlingRights &= side switch
            {
                Side.White => ~CastlingRights.WhiteBoth,
                Side.Black => ~CastlingRights.BlackBoth,
                _ => throw new ArgumentOutOfRangeException(nameof(side))
            };

            return castlingRights;
        }

        public static CastlingRights RemoveKingSide(this CastlingRights castlingRights, Side side)
        {
            castlingRights &= side switch
            {
                Side.White => ~CastlingRights.WhiteKingSide,
                Side.Black => ~CastlingRights.BlackKingSide,
                _ => throw new ArgumentOutOfRangeException(nameof(side))
            };

            return castlingRights;
        }

        public static CastlingRights RemoveQueenSide(this CastlingRights castlingRights, Side side)
        {
            castlingRights &= side switch
            {
                Side.White => ~CastlingRights.WhiteQueenSide,
                Side.Black => ~CastlingRights.BlackQueenSide,
                _ => throw new ArgumentOutOfRangeException(nameof(side))
            };

            return castlingRights;
        }
    }
}
