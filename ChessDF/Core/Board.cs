using ChessDF.Exceptions;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Core
{
    public class Board : IEquatable<Board?>
    {
        private readonly Bitboard[,] _pieceBB = new Bitboard[2, 6];
        
        public Bitboard WhitePawns { get => _pieceBB[0, 0]; set => _pieceBB[0, 0] = value; }
        public Bitboard WhiteKnights { get => _pieceBB[0, 1]; set => _pieceBB[0, 1] = value; }
        public Bitboard WhiteBishops { get => _pieceBB[0, 2]; set => _pieceBB[0, 2] = value; }
        public Bitboard WhiteRooks { get => _pieceBB[0, 3]; set => _pieceBB[0, 3] = value; }
        public Bitboard WhiteQueens { get => _pieceBB[0, 4]; set => _pieceBB[0, 4] = value; }
        public Bitboard WhiteKing { get => _pieceBB[0, 5]; set => _pieceBB[0, 5] = value; }

        public Bitboard BlackPawns { get => _pieceBB[1, 0]; set => _pieceBB[1, 0] = value; }
        public Bitboard BlackKnights { get => _pieceBB[1, 1]; set => _pieceBB[1, 1] = value; }
        public Bitboard BlackBishops { get => _pieceBB[1, 2]; set => _pieceBB[1, 2] = value; }
        public Bitboard BlackRooks { get => _pieceBB[1, 3]; set => _pieceBB[1, 3] = value; }
        public Bitboard BlackQueens { get => _pieceBB[1, 4]; set => _pieceBB[1, 4] = value; }
        public Bitboard BlackKing { get => _pieceBB[1, 5]; set => _pieceBB[1, 5] = value; }

        public Bitboard WhitePieces => WhitePawns | WhiteKnights | WhiteBishops | WhiteRooks | WhiteQueens | WhiteKing;
        public Bitboard BlackPieces => BlackPawns | BlackKnights | BlackBishops | BlackRooks | BlackQueens | BlackKing;
        public Bitboard OccupiedSquares => WhitePieces | BlackPieces;
        public Bitboard EmptySquares => ~OccupiedSquares;

        public Board() { }

        private Board(Bitboard[,] pieceBB)
        {
            _pieceBB = pieceBB;
        }

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

        public Bitboard this[Side side, Piece piece]
        {
            get => _pieceBB[(int)side, (int)piece];
            set => _pieceBB[(int)side, (int)piece] = value;
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

        public (Side side, Piece piece) GetPieceOnSquare(Square square)
        {
            for (int s = 0; s < 2; s++)
            {
                for (int p = 0; p < 6; p++)
                {
                    if (((_pieceBB[s, p] >> (int)square) & 1) == 1)
                        return ((Side)s, (Piece)p);
                }
            }

            throw new PieceNotFoundException();
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

        public Board Copy() => new Board((Bitboard[,])_pieceBB.Clone());

        public static Board FromPiecePlacementString(string piecePlacement)
        {
            // Replace digits with 0s for empty squares
            piecePlacement = piecePlacement.Replace("1", "0");
            piecePlacement = piecePlacement.Replace("2", "00");
            piecePlacement = piecePlacement.Replace("3", "000");
            piecePlacement = piecePlacement.Replace("4", "0000");
            piecePlacement = piecePlacement.Replace("5", "00000");
            piecePlacement = piecePlacement.Replace("6", "000000");
            piecePlacement = piecePlacement.Replace("7", "0000000");
            piecePlacement = piecePlacement.Replace("8", "00000000");

            string[] ranks = piecePlacement.Split('/');

            if (ranks.Length != 8)
                throw new ArgumentException("Invalid rank count", nameof(piecePlacement));

            if (ranks.Any(r => r.Length != 8))
                throw new ArgumentException("Piece placement contains invalid ranks", nameof(piecePlacement));

            var board = new Board();

            for (int i = 0; i < 64; i++)
            {
                char pieceChar = ranks[7 - i / 8][i % 8];

                (Side side, Piece piece)? pieceAndSide = pieceChar switch
                {
                    '0' => null,
                    'P' => (Side.White, Piece.Pawn),
                    'N' => (Side.White, Piece.Knight),
                    'B' => (Side.White, Piece.Bishop),
                    'R' => (Side.White, Piece.Rook),
                    'Q' => (Side.White, Piece.Queen),
                    'K' => (Side.White, Piece.King),
                    'p' => (Side.Black, Piece.Pawn),
                    'n' => (Side.Black, Piece.Knight),
                    'b' => (Side.Black, Piece.Bishop),
                    'r' => (Side.Black, Piece.Rook),
                    'q' => (Side.Black, Piece.Queen),
                    'k' => (Side.Black, Piece.King),
                    _ => throw new ArgumentException($"Piece placement contains invalid character: {pieceChar}", nameof(piecePlacement))
                };

                if (pieceAndSide is not null)
                {
                    Bitboard squareBB = (ulong)1 << i;
                    board[pieceAndSide.Value.side, pieceAndSide.Value.piece] |= squareBB;
                }
            }

            return board;
        }

        public string ToPiecePlacementString()
        {
            var outputString = new StringBuilder();

            for (int r = 7; r >= 0; r--)
            {
                for (int f = 0; f < 8; f++)
                {
                    int index = r * 8 + f;

                    Bitboard sq = (ulong)1 << index;
                    if ((sq & OccupiedSquares) == 0)
                    {
                        if (int.TryParse(outputString[^1].ToString(), out int emptyNum))
                            outputString[^1] = (emptyNum + 1).ToString()[0];
                        else
                            outputString.Append('1');

                        continue;
                    }

                    (Side side, Piece piece) = GetPieceOnSquare((Square)index);

                    char pieceChar = piece switch
                    {
                        Piece.Pawn => 'P',
                        Piece.Knight => 'N',
                        Piece.Bishop => 'B',
                        Piece.Rook => 'R',
                        Piece.Queen => 'Q',
                        Piece.King => 'K',
                        _ => throw new IndexOutOfRangeException(nameof(piece))
                    };

                    if (side == Side.Black)
                        pieceChar = char.ToLower(pieceChar);

                    outputString.Append(pieceChar);
                }

                if (r > 0)
                    outputString.Append('/');
            }

            return outputString.ToString();
        }

        public override string ToString()
        {
            return ToPiecePlacementString();
        }
    }
}
