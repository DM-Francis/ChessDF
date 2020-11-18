using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Core
{
    public struct Board
    {
        public Bitboard WhitePawns { get; set; }
        public Bitboard WhiteKnights { get; set; }
        public Bitboard WhiteBishops { get; set; }
        public Bitboard WhiteRooks { get; set; }
        public Bitboard WhiteQueens { get; set; }
        public Bitboard WhiteKing { get; set; }

        public Bitboard BlackPawns { get; set; }
        public Bitboard BlackKnights { get; set; }
        public Bitboard BlackBishops { get; set; }
        public Bitboard BlackRooks { get; set; }
        public Bitboard BlackQueens { get; set; }
        public Bitboard BlackKing { get; set; }

        public static Board StartingPosition => new Board
        {
            WhitePawns = new Bitboard(0x00_00_00_00_00_00_FF_00),
            WhiteKnights = new Bitboard(0x00_00_00_00_00_00_00_42),
            WhiteBishops = new Bitboard(0x00_00_00_00_00_00_00_24),
            WhiteRooks = new Bitboard(0x00_00_00_00_00_00_00_81),
            WhiteQueens = new Bitboard(0x00_00_00_00_00_00_00_08),
            WhiteKing = new Bitboard(0x00_00_00_00_00_00_00_10),

            BlackPawns = new Bitboard(0x00_FF_00_00_00_00_00_00),
            BlackKnights = new Bitboard(0x42_00_00_00_00_00_00_00),
            BlackBishops = new Bitboard(0x24_00_00_00_00_00_00_00),
            BlackRooks = new Bitboard(0x81_00_00_00_00_00_00_00),
            BlackQueens = new Bitboard(0x08_00_00_00_00_00_00_00),
            BlackKing = new Bitboard(0x10_00_00_00_00_00_00_00)
        };
    }
}
