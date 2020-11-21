using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Core
{
    public class Board
    {
        private readonly Bitboard[,] _pieceBB = new Bitboard[2, 6];
        public Bitboard EnpassantSquare { get; set; }

        public Bitboard WhitePawns { get => _pieceBB[0, 0]; init => _pieceBB[0, 0] = value; }
        public Bitboard WhiteKnights { get => _pieceBB[0, 1]; init => _pieceBB[0, 1] = value; }
        public Bitboard WhiteBishops { get => _pieceBB[0, 2]; init => _pieceBB[0, 2] = value; }
        public Bitboard WhiteRooks { get => _pieceBB[0, 3]; init => _pieceBB[0, 3] = value; }
        public Bitboard WhiteQueens { get => _pieceBB[0, 4]; init => _pieceBB[0, 4] = value; }
        public Bitboard WhiteKing { get => _pieceBB[0, 5]; init => _pieceBB[0, 5] = value; }

        public Bitboard BlackPawns { get => _pieceBB[1, 0]; init => _pieceBB[1, 0] = value; }
        public Bitboard BlackKnights { get => _pieceBB[1, 1]; init => _pieceBB[1, 1] = value; }
        public Bitboard BlackBishops { get => _pieceBB[1, 2]; init => _pieceBB[1, 2] = value; }
        public Bitboard BlackRooks { get => _pieceBB[1, 3]; init => _pieceBB[1, 3] = value; }
        public Bitboard BlackQueens { get => _pieceBB[1, 4]; init => _pieceBB[1, 4] = value; }
        public Bitboard BlackKing { get => _pieceBB[1, 5]; init => _pieceBB[1, 5] = value; }

        public Bitboard WhitePieces => WhitePawns | WhiteKnights | WhiteBishops | WhiteRooks | WhiteQueens | WhiteKing;
        public Bitboard BlackPieces => BlackPawns | BlackKnights | BlackBishops | BlackRooks | BlackQueens | BlackKing;
        public Bitboard OccupiedSquares => WhitePieces | BlackPieces;
        public Bitboard EmptySquares => ~OccupiedSquares;

        public Bitboard this[Side side, Piece piece]
        {
            get
            {
                int sideIndex = (int)side;
                return piece switch
                {
                    Piece.Pawn => _pieceBB[sideIndex, 0],
                    Piece.Knight => _pieceBB[sideIndex, 1],
                    Piece.Bishop => _pieceBB[sideIndex, 2],
                    Piece.Rook => _pieceBB[sideIndex, 3],
                    Piece.Queen => _pieceBB[sideIndex, 4],
                    Piece.King => _pieceBB[sideIndex, 5],
                    _ => throw new ArgumentOutOfRangeException(nameof(piece))
                };
            }
        }

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
