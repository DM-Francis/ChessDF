using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    public static class PawnMoves
    {
        public static Bitboard SinglePawnPushTargets(this Board board, Side side)
        {
            return side switch
            {
                Side.White => board.WhitePawns.NortOne() & board.EmptySquares,
                Side.Black => board.BlackPawns.SoutOne() & board.EmptySquares,
                _ => throw new ArgumentOutOfRangeException(nameof(side))
            };
        }

        public static Bitboard DoublePawnPushTargets(this Board board, Side side)
        {
            Bitboard singlePushes = SinglePawnPushTargets(board, side);

            switch (side)
            {
                case Side.White:
                    Bitboard rank4 = 0x00000000FF000000;
                    return singlePushes.NortOne() & board.EmptySquares & rank4;
                case Side.Black:
                    Bitboard rank5 = 0x000000FF00000000;
                    return singlePushes.SoutOne() & board.EmptySquares & rank5;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side));
            }
        }

        public static Bitboard WhitePawnEastAttacks(this Board board) => board.WhitePawns.NoEaOne();
        public static Bitboard WhitePawnWestAttacks(this Board board) => board.WhitePawns.NoWeOne();
        public static Bitboard BlackPawnEastAttacks(this Board board) => board.BlackPawns.SoEaOne();
        public static Bitboard BlackPawnWestAttacks(this Board board) => board.BlackPawns.SoWeOne();
    }
}
