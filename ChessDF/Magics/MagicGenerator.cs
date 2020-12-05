using ChessDF.Core;
using ChessDF.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Magics
{
    public class MagicGenerator
    {
        private static readonly Random _rng = new();
        private static readonly Bitboard AFile = 0x01_01_01_01_01_01_01_01;
        private static readonly Bitboard HFile = 0x80_80_80_80_80_80_80_80;
        private static readonly Bitboard FirstRank = 0x00_00_00_00_00_00_00_ff;
        private static readonly Bitboard EighthRank = 0xff_00_00_00_00_00_00_00;

        public static ulong RandomULong()
        {
            byte[] bytes = new byte[8];
            _rng.NextBytes(bytes);
            return BitConverter.ToUInt64(bytes);
        }

        public static ulong RandomULongFewBits() => RandomULong() & RandomULong() & RandomULong();

        public static Bitboard RookMask(Square square)
        {
            Bitboard result = Rays.RookAttacks(square);
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

        public static Bitboard BishopMask(Square square)
        {
            Bitboard result = Rays.BishopAttacks(square);
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

        public static (Bitboard magic, int indexSize) FindMagic(Square square, int trys, Piece piece)
        {
            Bitboard mask = piece switch
            {
                Piece.Bishop => BishopMask(square),
                Piece.Rook => RookMask(square),
                _ => throw new ArgumentOutOfRangeException(nameof(piece), $"{nameof(piece)} must be either a bishop or rook.")
            };
            int indexSize = mask.PopCount();
            Bitboard[] occupancies = GetAllPossibleOccupanciesForMask(mask);
            Bitboard[] attacks = new Bitboard[occupancies.Length];
            Bitboard[] used = new Bitboard[occupancies.Length];

            for (int i = 0; i < occupancies.Length; i++)
            {
                attacks[i] = piece switch
                {
                    Piece.Bishop => Rays.BishopAttacks(square, occupancies[i]),
                    Piece.Rook => Rays.RookAttacks(square, occupancies[i]),
                    _ => throw new ArgumentOutOfRangeException(nameof(piece), $"{nameof(piece)} must be either a bishop or rook.")
                };
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
                    int j = TransformOccupiedToIndex(occupancies[i], magic, indexSize);

                    if (used[j] == 0)
                        used[j] = attacks[i];
                    else if (used[j] != attacks[i])
                        failed = true;

                    if (failed)
                        break;
                }

                if (!failed)
                    return (magic, indexSize);
            }

            throw new MagicNotFoundException();
        }

        public static int TransformOccupiedToIndex(Bitboard occupied, Bitboard magic, int bits)
        {
            return (int)((occupied * magic) >> (64 - bits));
        }

        public static void GenerateMagicsToFile(string targetFile)
        {
            var allMagics = new List<MagicDetails>();
            for (int s = 0; s < 64; s++)
            {
                (Bitboard magic, int bitCount) = FindMagic((Square)s, 100_000_000, Piece.Bishop);
                allMagics.Add(new MagicDetails(magic, bitCount, Piece.Bishop, (Square)s));
            }

            for (int s = 0; s < 64; s++)
            {
                (Bitboard magic, int bitCount) = FindMagic((Square)s, 100_000_000, Piece.Rook);
                allMagics.Add(new MagicDetails(magic, bitCount, Piece.Rook, (Square)s));
            }

            using var writer = new StreamWriter(targetFile);

            foreach(var magicDetail in allMagics)
            {
                writer.WriteLine(magicDetail.ToString());
            }
        }

        public static List<MagicDetails> LoadMagics()
        {
            var assembly = Assembly.GetAssembly(typeof(MagicGenerator));
            Stream? magicFileStream = assembly?.GetManifestResourceStream("ChessDF.magics.txt");

            if (magicFileStream is null)
                throw new FileNotFoundException("Could not find embedded magics file");

            var output = new List<MagicDetails>();

            using var reader = new StreamReader(magicFileStream);

            string? line;
            while ((line = reader.ReadLine()) is not null)
            {
                output.Add(MagicDetails.FromString(line));
            }

            return output;
        }

        public static Bitboard[] CreateAttacksMapForMagic(MagicDetails details)
        {
            Bitboard magic = details.Magic;
            Piece piece = details.Piece;
            Square square = details.Square;

            Bitboard mask = piece switch
            {
                Piece.Bishop => BishopMask(square),
                Piece.Rook => RookMask(square),
                _ => throw new IndexOutOfRangeException($"{nameof(piece)} must be either a bishop or rook.")
            };
            int indexSize = mask.PopCount();
            Bitboard[] occupancies = GetAllPossibleOccupanciesForMask(mask);
            Bitboard[] attacks = new Bitboard[occupancies.Length];
            Bitboard[] output = new Bitboard[occupancies.Length];

            for (int i = 0; i < occupancies.Length; i++)
            {
                attacks[i] = piece switch
                {
                    Piece.Bishop => Rays.BishopAttacks(square, occupancies[i]),
                    Piece.Rook => Rays.RookAttacks(square, occupancies[i]),
                    _ => throw new ArgumentOutOfRangeException(nameof(piece), $"{nameof(piece)} must be either a bishop or rook.")
                };
            }

            for (int i = 0; i < occupancies.Length; i++)
            {
                int j = TransformOccupiedToIndex(occupancies[i], magic, indexSize);

                output[j] = attacks[i];
            }

            return output;
        }
    }
}
