using ChessDF.Core;
using ChessDF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    public static class SlidingPieceMoves
    {
        private static readonly Bitboard[] _northRays = new Bitboard[64];
        private static readonly Bitboard[] _southRays = new Bitboard[64];
        private static readonly Bitboard[] _eastRays = new Bitboard[64];
        private static readonly Bitboard[] _westRays = new Bitboard[64];
        private static readonly Bitboard[] _northEastRays = new Bitboard[64];
        private static readonly Bitboard[] _northWestRays = new Bitboard[64];
        private static readonly Bitboard[] _southEastRays = new Bitboard[64];
        private static readonly Bitboard[] _southWestRays = new Bitboard[64];

        public static Bitboard RayAttacks(Square square, Direction direction)
        {
            int sq = (int)square;
            return direction switch
            {
                Direction.North => _northRays[sq],
                Direction.South => _southRays[sq],
                Direction.East => _eastRays[sq],
                Direction.West => _westRays[sq],
                Direction.NorthEast => _northEastRays[sq],
                Direction.NorthWest => _northWestRays[sq],
                Direction.SouthEast => _southEastRays[sq],
                Direction.SouthWest => _southWestRays[sq],
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        private static Bitboard GetRayAttacks(Bitboard occupied, Direction direction, Square square)
        {
            Bitboard attacks = RayAttacks(square, direction);
            Bitboard blockers = attacks & occupied;

            if (blockers > 0)
            {
                int blockerSquare = IsNegative(direction) ? BitUtils.BitScanReverse(blockers) : BitUtils.BitScanForward(blockers);
                attacks ^= RayAttacks((Square)blockerSquare, direction);
            }

            return attacks;
        }

        private static bool IsNegative(Direction direction) => direction is
            Direction.West or
            Direction.SouthWest or
            Direction.South or
            Direction.SouthEast;

        public static Bitboard RankAttacks(Square square) => _eastRays[(int)square] | _westRays[(int)square];
        public static Bitboard FileAttacks(Square square) => _northRays[(int)square] | _southRays[(int)square];
        public static Bitboard DiagonalAttacks(Square square) => _northEastRays[(int)square] | _southWestRays[(int)square];
        public static Bitboard AntiDiagonalAttacks(Square square) => _northWestRays[(int)square] | _southEastRays[(int)square];


        public static Bitboard RankAttacks(Bitboard occupied, Square square)
        {
            return GetRayAttacks(occupied, Direction.East, square) | GetRayAttacks(occupied, Direction.West, square);
        }

        public static Bitboard FileAttacks(Bitboard occupied, Square square)
        {
            return GetRayAttacks(occupied, Direction.North, square) | GetRayAttacks(occupied, Direction.South, square);
        }

        public static Bitboard DiagonalAttacks(Bitboard occupied, Square square)
        {
            return GetRayAttacks(occupied, Direction.NorthEast, square) | GetRayAttacks(occupied, Direction.SouthWest, square);
        }

        public static Bitboard AntiDiagonalAttacks(Bitboard occupied, Square square)
        {
            return GetRayAttacks(occupied, Direction.NorthWest, square) | GetRayAttacks(occupied, Direction.SouthEast, square);
        }

        public static Bitboard RookAttacks(Square square) => RankAttacks(square) | FileAttacks(square);
        public static Bitboard BishopAttacks(Square square) => DiagonalAttacks(square) | AntiDiagonalAttacks(square);
        public static Bitboard QueenAttacks(Square square) => RookAttacks(square) | BishopAttacks(square);

        public static Bitboard RookAttacks(Bitboard occupied, Square square) => RankAttacks(occupied, square) | FileAttacks(occupied, square);
        public static Bitboard BishopAttacks(Bitboard occupied, Square square) => DiagonalAttacks(occupied, square) | AntiDiagonalAttacks(occupied, square);
        public static Bitboard QueenAttacks(Bitboard occupied, Square square) => RookAttacks(occupied, square) | BishopAttacks(occupied, square);

        static SlidingPieceMoves()
        {
            GenerateNorthRays();
            GenerateSouthRays();
            GenerateEastRays();
            GenerateWestRays();
            GenerateNorthEastRays();
            GenerateNorthWestRays();
            GenerateSouthEastRays();
            GenerateSouthWestRays();
        }

        private static void GenerateNorthRays()
        {
            ulong north = 0x0101010101010100;
            for (int sq = 0; sq < 64; sq++, north <<= 1)
                _northRays[sq] = north;
        }

        private static void GenerateSouthRays()
        {
            ulong south = 0x00_80_80_80_80_80_80_80;
            for (int sq = 63; sq >= 0; sq--, south >>= 1)
                _southRays[sq] = south;
        }

        private static void GenerateEastRays()
        {
            Bitboard eastBase = 0x00_00_00_00_00_00_00_fe;
            for (int file = 0; file < 8; file++, eastBase = eastBase.EastOne())
            {
                Bitboard east = eastBase;
                for (int rank = 0; rank < 8; rank++, east = east.NortOne())
                    _eastRays[rank * 8 + file] = east;
            }
        }

        private static void GenerateWestRays()
        {
            Bitboard westBase = 0x00_00_00_00_00_00_00_7f;
            for (int file = 7; file >= 0; file--, westBase = westBase.WestOne())
            {
                Bitboard west = westBase;
                for (int rank = 0; rank < 8; rank++, west = west.NortOne())
                    _westRays[rank * 8 + file] = west;
            }
        }

        private static void GenerateNorthEastRays()
        {
            Bitboard northEastBase = 0x80_40_20_10_08_04_02_00;
            for (int file = 0; file < 8; file++, northEastBase = northEastBase.EastOne())
            {
                Bitboard northEast = northEastBase;
                for (int rank = 0; rank < 8; rank++, northEast = northEast.NortOne())
                    _northEastRays[rank * 8 + file] = northEast;
            }
        }

        private static void GenerateNorthWestRays()
        {
            Bitboard NorthWestBase = 0x01_02_04_08_10_20_40_00;
            for (int file = 7; file >= 0; file--, NorthWestBase = NorthWestBase.WestOne())
            {
                Bitboard northWest = NorthWestBase;
                for (int rank = 0; rank < 8; rank++, northWest = northWest.NortOne())
                    _northWestRays[rank * 8 + file] = northWest;
            }
        }

        private static void GenerateSouthEastRays()
        {
            Bitboard southEastBase = 0x00_02_04_08_10_20_40_80;
            for (int file = 0; file < 8; file++, southEastBase = southEastBase.EastOne())
            {
                Bitboard southEast = southEastBase;
                for (int rank = 7; rank >= 0; rank--, southEast = southEast.SoutOne())
                    _southEastRays[rank * 8 + file] = southEast;
            }
        }

        private static void GenerateSouthWestRays()
        {
            Bitboard southWestBase = 0x00_40_20_10_08_04_02_01;
            for (int file = 7; file >= 0; file--, southWestBase = southWestBase.WestOne())
            {
                Bitboard southWest = southWestBase;
                for (int rank = 7; rank >= 0; rank--, southWest = southWest.SoutOne())
                    _southWestRays[rank * 8 + file] = southWest;
            }
        }
    }
}
