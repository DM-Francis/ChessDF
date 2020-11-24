using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    public readonly struct Move : IEquatable<Move>
    {
        private readonly uint _moveData;

        public Move(Square from, Square to, MoveFlags flags)
        {
            _moveData = ((uint)flags & 0xf) << 12 | ((uint)from & 0x3f) << 6 | ((uint)to & 0x3f);
        }

        public Square From => (Square)((_moveData >> 6) & 0x3f);
        public Square To => (Square)(_moveData & 0x3f);
        public MoveFlags Flags => (MoveFlags)((_moveData >> 12) & 0xf);

        public bool IsCapture => (Flags & MoveFlags.Capture) != 0;
        public bool IsPromotion => (Flags & MoveFlags.KnightPromotion) != 0;
        public bool IsCastle => Flags == MoveFlags.KingCastle || Flags == MoveFlags.QueenCastle;
        
        public Piece? PromotionPiece => (Flags & ~MoveFlags.Capture) switch
        {
            MoveFlags.KnightPromotion => Piece.Knight,
            MoveFlags.BishopPromotion => Piece.Bishop,
            MoveFlags.RookPromotion => Piece.Rook,
            MoveFlags.QueenPromotion => Piece.Queen,
            _ => null
        };

        public override bool Equals(object? obj) => obj is Move move && Equals(move);
        public bool Equals(Move other) => _moveData == other._moveData;
        public override int GetHashCode() => HashCode.Combine(_moveData);
        public static bool operator ==(Move left, Move right) => left.Equals(right);
        public static bool operator !=(Move left, Move right) => !(left == right);
        public override string ToString() => IsCapture ? $"{From}x{To}" : $"{From}->{To}";

        public static Move FromStringAndPosition(string moveString, Position position)
        {
            var moveStringRegex = new Regex("[a-h][1-8][a-h][1-8][nbrq]?");
            if (!moveStringRegex.IsMatch(moveString))
                throw new ArgumentException($"Invalid move string: '{moveString}'", nameof(moveString));

            Board board = position.Board;

            Square from = Enum.Parse<Square>(moveString.Substring(0, 2));
            Square to = Enum.Parse<Square>(moveString.Substring(2, 2));
            (Side fromSide, Piece fromPiece) = board.GetPieceOnSquare(from);

            // Check for castling
            if (fromPiece == Piece.King)
            {
                if (from == Square.e1 && to == Square.g1 || from == Square.e8 && to == Square.g8)
                    return new Move(from, to, MoveFlags.KingCastle);
                else if (from == Square.e1 && to == Square.c1 || from == Square.e8 && to == Square.c8)
                    return new Move(from, to, MoveFlags.QueenCastle);
            }

            // Check for en passant
            if (to == position.EnPassantSquare && fromPiece == Piece.Pawn)
                return new Move(from, to, MoveFlags.EnPassantCapture);

            // Check for double pawn push
            if (fromPiece == Piece.Pawn && Math.Abs(from - to) == 16)
                return new Move(from, to, MoveFlags.DoublePawnPush);

            MoveFlags flags = MoveFlags.QuietMove;
            if (moveString.Length == 5)
            {
                flags = moveString[4] switch
                {
                    'n' => MoveFlags.KnightPromotion,
                    'b' => MoveFlags.BishopPromotion,
                    'r' => MoveFlags.RookPromotion,
                    'q' => MoveFlags.QueenPromotion,
                    _ => throw new ArgumentException($"Invalid promotion piece in move string '{moveString[4]}'")
                };
            }

            // Check for captures
            if ((Bitboard.FromSquare(to) & board.OpposingPieces(fromSide)) != 0)
                flags |= MoveFlags.Capture;

            return new Move(from, to, flags);
        }
    }
}
