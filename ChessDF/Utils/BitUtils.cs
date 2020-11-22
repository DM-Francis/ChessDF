using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Utils
{
    internal static class BitUtils
    {
        private static readonly int[] _index64Forward = new int[64]
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

        private static readonly int[] _index64Reverse = new int[64]
        {
            0, 47,  1, 56, 48, 27,  2, 60,
            57, 49, 41, 37, 28, 16,  3, 61,
            54, 58, 35, 52, 50, 42, 21, 44,
            38, 32, 29, 23, 17, 11,  4, 62,
            46, 55, 26, 59, 40, 36, 15, 53,
            34, 51, 20, 43, 31, 22, 10, 45,
            25, 39, 14, 33, 19, 30,  9, 24,
            13, 18,  8, 12,  7,  6,  5, 63
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
            return _index64Forward[((bits & (0 - bits)) * debruijn64) >> 58];
        }

        /// <summary>
        /// Using de Bruijn Sequences to Index a 1 in a Computer Word
        /// </summary>
        /// <param name="bits">Bitboard to scan</param>
        /// <returns>Index (0..63) of most significant one bit</returns>
        public static int BitScanReverse(ulong bits)
        {
            if (bits == 0)
                throw new ArgumentException($"{nameof(bits)} cannot be zero.", nameof(bits));

            ulong debruijn64 = 0x03f79d71b4cb0a89;
            bits |= bits >> 1;
            bits |= bits >> 2;
            bits |= bits >> 4;
            bits |= bits >> 8;
            bits |= bits >> 16;
            bits |= bits >> 32;
            return _index64Reverse[(bits * debruijn64) >> 58];
        }

        public static bool IsASubsetOfB(ulong a, ulong b) => (a & b) == a;
    }
}
