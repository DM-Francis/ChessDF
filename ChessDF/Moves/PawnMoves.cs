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
        private static readonly Bitboard[] _whitePawnAttacks = new Bitboard[64];
        private static readonly Bitboard[] _blackPawnAttacks = new Bitboard[64];

        static PawnMoves()
        {
            for (int i = 8; i < 56; i++)
            {
                Bitboard singlePawn = (ulong)1 << i;
                _whitePawnAttacks[i] = singlePawn.NoEaOne() | singlePawn.NoWeOne();
                _blackPawnAttacks[i] = singlePawn.SoEaOne() | singlePawn.SoWeOne();
            }
        }

        public static Bitboard WhitePawnAttacks(Square square)
        {
            if (square < Square.a2)
                throw new ArgumentOutOfRangeException(nameof(square), "Pawn cannot be on rank 1");

            if (square >= Square.a8)
                throw new ArgumentOutOfRangeException(nameof(square), "Pawn cannot be on rank 8");

            return _whitePawnAttacks[(int)square];
        }

        public static Bitboard BlackPawnAttacks(Square square)
        {
            if (square < Square.a2)
                throw new ArgumentOutOfRangeException(nameof(square), "Pawn cannot be on rank 1");

            if (square >= Square.a8)
                throw new ArgumentOutOfRangeException(nameof(square), "Pawn cannot be on rank 8");

            return _blackPawnAttacks[(int)square];
        }


        public static Bitboard PawnSinglePushTargets(Bitboard pawns, Bitboard emptySquares, Side side)
        {
            return side switch
            {
                Side.White => pawns.NortOne() & emptySquares,
                Side.Black => pawns.SoutOne() & emptySquares,
                _ => throw new ArgumentOutOfRangeException(nameof(side))
            };
        }

        public static Bitboard PawnDoublePushTargets(Bitboard pawns, Bitboard emptySquares, Side side)
        {
            Bitboard singlePushes = PawnSinglePushTargets(pawns, emptySquares, side);

            switch (side)
            {
                case Side.White:
                    Bitboard rank4 = 0x00000000FF000000;
                    return singlePushes.NortOne() & emptySquares & rank4;
                case Side.Black:
                    Bitboard rank5 = 0x000000FF00000000;
                    return singlePushes.SoutOne() & emptySquares & rank5;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side));
            }
        }
    }
}
