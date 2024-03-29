﻿using ChessDF.Core;
using System;
using System.Text.RegularExpressions;

namespace ChessDF.Moves
{
    public readonly struct Move : IEquatable<Move>
    {
        private readonly uint _moveData;

        public Move(Square from, Square to, MoveFlags flags)
        {
            _moveData = ((uint)flags & 0xf) << 12 | ((uint)from & 0x3f) << 6 | ((uint)to & 0x3f);
            if (IsCapture)
                throw new ArgumentException("Use the Move constructor with the captured piece parameter for captures");
        }

        public Move(Square from, Square to, MoveFlags flags, Piece capturedPiece)
        {
            _moveData = ((uint)capturedPiece & 0x7) << 16 | ((uint)flags & 0xf) << 12 | ((uint)from & 0x3f) << 6 | ((uint)to & 0x3f);
            if (!IsCapture)
                throw new ArgumentException("Do not specifiy a captured piece for non-capturing moves");
        }

        public Square From => (Square)((_moveData >> 6) & 0x3f);
        public Square To => (Square)(_moveData & 0x3f);
        public MoveFlags Flags => (MoveFlags)((_moveData >> 12) & 0xf);
        public Piece? CapturedPiece => IsCapture ? (Piece)((_moveData >> 16) & 0x7) : null;

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

        public string ToUciMoveString()
        {
            if (IsPromotion)
            {
                char promoChar = PromotionPiece switch
                {
                    Piece.Bishop => 'b',
                    Piece.Knight => 'n',
                    Piece.Rook => 'r',
                    Piece.Queen => 'q',
                    _ => throw new InvalidOperationException($"Invalid piece for promotion '{PromotionPiece}'")
                };

                return $"{From}{To}{promoChar}";
            }

            return $"{From}{To}";
        }

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
                return new Move(from, to, MoveFlags.EnPassantCapture, Piece.Pawn);

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
            Bitboard captureSquare = Bitboard.FromSquare(to) & board.OpposingPieces(fromSide);
            if (captureSquare != 0)
            {
                flags |= MoveFlags.Capture;
                var sq = (Square)captureSquare.FirstIndex();
                (_, Piece piece) = board.GetPieceOnSquare(sq);
                return new Move(from, to, flags, piece);
            }

            return new Move(from, to, flags);
        }
    }
}
