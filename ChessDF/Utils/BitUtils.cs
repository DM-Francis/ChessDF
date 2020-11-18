using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Utils
{
    internal static class BitUtils
    {
        private static int[] _index64 = new int[64]
        {
            0,  1, 48,  2, 57, 49, 28,  3,
            61, 58, 50, 42, 38, 29, 17,  4,
            62, 55, 59, 36, 53, 51, 43, 22,
            45, 39, 33, 30, 24, 18, 12,  5,
            63, 47, 56, 27, 60, 41, 37, 16,
            54, 35, 52, 21, 44, 32, 23, 11,
            46, 26, 40, 15, 34, 20, 31, 10,
            25, 14, 19,  9, 13,  8,  7,  6
        };

        /// <summary>
        /// Using de Bruijn Sequences to Index a 1 in a Computer Word
        /// </summary>
        /// <param name="bits">Bitboard to scan</param>
        /// <returns>Index (0..63) of least significant one bit</returns>
        public static int BitScanForward(ulong bits)
        {
            if (bits == 0)
                throw new ArgumentException($"{nameof(bits)} cannot be zero.", nameof(bits));

            ulong debruijn64 = 0x03f79d71b4cb0a89;
            return _index64[((bits & (0 - bits)) * debruijn64) >> 58];
        }
    }
}
