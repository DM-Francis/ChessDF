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
        private static readonly Bitboard _kingSideBetween = 0x60;
        private static readonly Bitboard _queenSideBetween = 0x0e;

        private static readonly Bitboard _kingSideChecks = 0x70;
        private static readonly Bitboard _queenSideChecks = 0x1c;

        private static readonly Bitboard _kingSideKingToFrom = 0x50;
        private static readonly Bitboard _kingSideRookToFrom = 0xa0;
        private static readonly Bitboard _queenSideKingToFrom = 0x00_00_00_00_00_00_00_50;
        private static readonly Bitboard _queenSideRookToFrom = 0x00_00_00_00_00_00_00_a0;

        public static Bitboard KingSideBetween(Side side) => side switch
        {
            Side.White => _kingSideBetween,
            Side.Black => _kingSideBetween.FlipVertical(),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static Bitboard QueenSideBetween(Side side) => side switch
        {
            Side.White => _queenSideBetween,
            Side.Black => _queenSideBetween.FlipVertical(),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static Bitboard KingSideChecks(Side side) => side switch
        {
            Side.White => _kingSideChecks,
            Side.Black => _kingSideChecks.FlipVertical(),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static Bitboard QueenSideChecks(Side side) => side switch
        {
            Side.White => _queenSideChecks,
            Side.Black => _queenSideChecks.FlipVertical(),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static Bitboard KingSideKingToFrom(Side side) => side switch
        {
            Side.White => _kingSideKingToFrom,
            Side.Black => _kingSideKingToFrom.FlipVertical(),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static Bitboard QueenSideKingToFrom(Side side) => side switch
        {
            Side.White => _queenSideKingToFrom,
            Side.Black => _queenSideKingToFrom.FlipVertical(),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static Bitboard KingSideRookToFrom(Side side) => side switch
        {
            Side.White => _kingSideRookToFrom,
            Side.Black => _kingSideRookToFrom.FlipVertical(),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static Bitboard QueenSideRookToFrom(Side side) => side switch
        {
            Side.White => _queenSideRookToFrom,
            Side.Black => _queenSideRookToFrom.FlipVertical(),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public static bool CanCastleEither(CastlingRights castlingRights, Side side) => side switch
        {
            Side.White => (castlingRights & CastlingRights.WhiteBoth) != 0,
            Side.Black => (castlingRights & CastlingRights.BlackBoth) != 0,
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
