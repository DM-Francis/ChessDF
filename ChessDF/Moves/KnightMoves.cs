using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    public static class KnightMoves
    {
        private static readonly Bitboard[] _knightAttacks = new Bitboard[64];

        private const ulong _notAFile = 0xfe_fe_fe_fe_fe_fe_fe_fe;
        private const ulong _notABFile = 0xfc_fc_fc_fc_fc_fc_fc_fc;
        private const ulong _notHFile = 0x7f_7f_7f_7f_7f_7f_7f_7f;
        private const ulong _notGHFile = 0x3f_3f_3f_3f_3f_3f_3f_3f;

        static KnightMoves()
        {
            for (int i = 0; i < 64; i++)
            {
                Bitboard k = (ulong)1 << i;
                _knightAttacks[i] =
                    (k << 17) & _notAFile |
                    (k << 10) & _notABFile |
                    (k >> 6) & _notABFile |
                    (k >> 15) & _notAFile |
                    (k << 15) & _notHFile |
                    (k << 6) & _notGHFile |
                    (k >> 10) & _notGHFile |
                    (k >> 17) & _notHFile;
            }
        }

        public static Bitboard KnightAttacks(Square square) => _knightAttacks[(int)square];
    }
}
