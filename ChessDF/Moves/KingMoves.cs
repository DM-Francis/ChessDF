using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    public static class KingMoves
    {
        private static readonly Bitboard[] _kingAttacks = new Bitboard[64];

        static KingMoves()
        {
            for (int i = 0; i < 64; i++)
            {
                Bitboard k = (ulong)1 << i;
                _kingAttacks[i] = KingAttacks(k);
            }
        }

        public static Bitboard KingAttacks(Bitboard king)
        {
            Bitboard attacks = king.EastOne() | king.WestOne();
            king |= attacks;
            attacks |= king.NortOne() | king.SoutOne();
            return attacks;
        }

        public static Bitboard KingAttacks(Square square) => _kingAttacks[(int)square];
    }
}
