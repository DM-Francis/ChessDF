using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    public static class Castling
    {
        public static readonly Bitboard WhiteKingSideBetween = 0x00_00_00_00_00_00_00_60;
        public static readonly Bitboard WhiteQueenSideBetween = 0x00_00_00_00_00_00_00_0e;
        public static readonly Bitboard BlackKingSideBetween = 0x60_00_00_00_00_00_00_00;
        public static readonly Bitboard BlackQueenSideBetween = 0x0e_00_00_00_00_00_00_00;

        public static readonly Bitboard WhiteKingSideChecks = 0x00_00_00_00_00_00_00_70;
        public static readonly Bitboard WhiteQueenSideChecks = 0x00_00_00_00_00_00_00_1c;
        public static readonly Bitboard BlackKingSideChecks = 0x70_00_00_00_00_00_00_00;
        public static readonly Bitboard BlackQueenSideChecks = 0x1c_00_00_00_00_00_00_00;

        public static Bitboard KingSideBetween(Side side) => side switch
        {
            Side.White => WhiteKingSideBetween,
            Side.Black => BlackKingSideBetween,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static Bitboard QueenSideBetween(Side side) => side switch
        {
            Side.White => WhiteQueenSideBetween,
            Side.Black => BlackQueenSideBetween,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static Bitboard KingSideChecks(Side side) => side switch
        {
            Side.White => WhiteKingSideChecks,
            Side.Black => BlackKingSideChecks,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static Bitboard QueenSideChecks(Side side) => side switch
        {
            Side.White => WhiteQueenSideChecks,
            Side.Black => BlackQueenSideChecks,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static bool CanCastleEither(CastlingRights castlingRights, Side side) => side switch
        {
            Side.White => (castlingRights & CastlingRights.White) != 0,
            Side.Black => (castlingRights & CastlingRights.Black) != 0,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static bool CanCastleKingside(CastlingRights castlingRights, Side side) => side switch
        {
            Side.White => (castlingRights & CastlingRights.WhiteKingSide) != 0,
            Side.Black => (castlingRights & CastlingRights.BlackKingSide) != 0,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static bool CanCastleQueenside(CastlingRights castlingRights, Side side) => side switch
        {
            Side.White => (castlingRights & CastlingRights.WhiteQueenSide) != 0,
            Side.Black => (castlingRights & CastlingRights.BlackQueenSide) != 0,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };
    }
}
