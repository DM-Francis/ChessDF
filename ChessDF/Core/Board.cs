using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Core
{
    public class Board : IEquatable<Board?>
    {
        private readonly Bitboard[,] _pieceBB = new Bitboard[2, 6];

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

        public Bitboard FriendlyPieces(Side side) => side switch
        {
            Side.White => WhitePieces,
            Side.Black => BlackPieces,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public Bitboard OpposingPieces(Side side) => side switch
        {
            Side.White => BlackPieces,
            Side.Black => WhitePieces,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        public Bitboard this[Side side, Piece piece] => _pieceBB[(int)side, (int)piece];

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

        public Bitboard AttacksBy(Side side)
        {
            Bitboard pawnAttacks = PawnMoves.AllPawnAttacks(this[side, Piece.Pawn], side);
            Bitboard knightAttacks = KnightMoves.AllKnightAttacks(this[side, Piece.Knight]);
            Bitboard bishopAttacks = SlidingPieceMoves.AllBishopAttacks(this[side, Piece.Bishop], this.OccupiedSquares);
            Bitboard rookAttacks = SlidingPieceMoves.AllRookAttacks(this[side, Piece.Rook], this.OccupiedSquares);
            Bitboard queenAttacks = SlidingPieceMoves.AllQueenAttacks(this[side, Piece.Queen], this.OccupiedSquares);
            Bitboard kingAttacks = KingMoves.KingAttacks(this[side, Piece.King]);

            return pawnAttacks | knightAttacks | bishopAttacks | rookAttacks | queenAttacks | kingAttacks;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Board);
        }

        public bool Equals(Board? other)
        {
            if (other is null)
                return false;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (_pieceBB[i, j] != other._pieceBB[i, j])
                        return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int blackPiecesHashcode = HashCode.Combine(BlackPawns, BlackKnights, BlackBishops, BlackRooks, BlackQueens, BlackKing);
            int whitePiecesHashcode = HashCode.Combine(WhitePawns, WhiteKnights, WhiteBishops, WhiteRooks, WhiteQueens, WhiteKing);

            return HashCode.Combine(blackPiecesHashcode, whitePiecesHashcode);
        }

        public static bool operator ==(Board? left, Board? right)
        {
            return EqualityComparer<Board>.Default.Equals(left, right);
        }

        public static bool operator !=(Board? left, Board? right)
        {
            return !(left == right);
        }
    }
}
