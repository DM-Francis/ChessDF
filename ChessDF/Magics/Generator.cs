using ChessDF.Core;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Magics
{
    public class Generator
    {
        private static readonly Random _rng = new();
        private static Bitboard AFile = 0x01_01_01_01_01_01_01_01;
        private static Bitboard HFile = 0x80_80_80_80_80_80_80_80;
        private static Bitboard FirstRank = 0x00_00_00_00_00_00_00_ff;
        private static Bitboard EighthRank = 0xff_00_00_00_00_00_00_00;

        public static ulong RandomULong()
        {
            byte[] bytes = new byte[8];
            _rng.NextBytes(bytes);
            return BitConverter.ToUInt64(bytes);
        }

        public static ulong RandomULongFewBits() => RandomULong() & RandomULong() & RandomULong();

        internal static Bitboard RookMask(Square square)
        {
            Bitboard result = SlidingPieceMoves.RookAttacks(square);
            int rank = (int)square / 8;
            int file = (int)square % 8;

            if (rank != 0)
                result &= ~FirstRank;
            if (rank != 7)
                result &= ~EighthRank;
            if (file != 0)
                result &= ~AFile;
            if (file != 7)
                result &= ~HFile;

            return result;
        }

        internal static Bitboard BishopMask(Square square)
        {
            Bitboard result = SlidingPieceMoves.BishopAttacks(square);
            int rank = (int)square / 8;
            int file = (int)square % 8;

            if (rank != 0)
                result &= ~FirstRank;
            if (rank != 7)
                result &= ~EighthRank;
            if (file != 0)
                result &= ~AFile;
            if (file != 7)
                result &= ~HFile;

            return result;
        }

        internal static Bitboard[] GetAllPossibleOccupanciesForMask(Bitboard mask)
        {
            int bitCount = mask.PopCount();
            var output = new Bitboard[(int)Math.Pow(2, bitCount)];

            for (int index = 0; index < output.Length; index++)
            {
                Bitboard occupancy = 0;
                int[] maskBits = mask.Serialize();
                for (int b = 0; b < bitCount; b++)
                {
                    ulong indexBit = (ulong)(index >> b) & 1;
                    occupancy ^= indexBit << maskBits[b];
                }

                output[index] = occupancy;
            }

            return output;
        }

        public static Bitboard FindMagic(Square square, int trys, bool bishop)
        {
            Bitboard mask = bishop ? BishopMask(square) : RookMask(square);
            int bitCount = mask.PopCount();
            Bitboard[] occupancies = GetAllPossibleOccupanciesForMask(mask);
            Bitboard[] attacks = new Bitboard[occupancies.Length];
            Bitboard[] used = new Bitboard[occupancies.Length];

            for (int i = 0; i < occupancies.Length; i++)
            {
                attacks[i] = bishop ? SlidingPieceMoves.BishopAttacks(square, occupancies[i]) : SlidingPieceMoves.RookAttacks(square, occupancies[i]);
            }

            for (int k = 0; k < trys; k++)
            {
                Bitboard magic = RandomULongFewBits();

                bool failed = false;
                for (int i = 0; i < occupancies.Length; i++)
                {
                    used[i] = 0;
                }
                for (int i = 0; i < occupancies.Length; i++)
                {
                    int j = TransformOccupiedToIndex(occupancies[i], magic, bitCount);

                    if (used[j] == 0)
                        used[j] = attacks[i];
                    else if (used[j] != attacks[i])
                        failed = true;

                    if (failed)
                        break;
                }

                if (!failed)
                    return magic;
            }

            return 0;
        }

        public static int TransformOccupiedToIndex(Bitboard occupied, Bitboard magic, int bits)
        {
            return (int)((occupied * magic) >> (64 - bits));
        }
    }
}
