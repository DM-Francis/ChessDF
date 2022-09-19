using ChessDF.Core;
using ChessDF.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    internal static class Mover
    {
        public static Position MakeMove(this Position position, Move move)
        {
            var newPosition = MakeMoveNoLegalCheck(position, move);
            bool isLegal = !KingIsInCheck(position.SideToMove, newPosition.Board);

            if (!isLegal)
                throw new IllegalMoveException("King is in check");

            return newPosition;
        }

        public static Position MakeMoveNoLegalCheck(this Position position, Move move)
        {
            Board board = position.Board;
            ApplyMoveToBoard(board, move, out Piece fromPiece);

            CastlingRights castlingRights = position.CastlingRights;
            int halfMoveClock = position.HalfmoveClock;
            Square? enPassantSquare = null;

            if (move.Flags == MoveFlags.DoublePawnPush)
            {
                enPassantSquare = position.SideToMove switch
                {
                    Side.White => move.From + 8,
                    Side.Black => move.From - 8,
                    _ => throw new IndexOutOfRangeException(nameof(position.SideToMove))
                };
            }

            if (fromPiece == Piece.King)
            {
                castlingRights = castlingRights.RemoveBoth(position.SideToMove);
            }

            castlingRights = UpdateCastlingRightsForBothSides(board, castlingRights);

            if (fromPiece == Piece.Pawn || move.IsCapture)
                halfMoveClock = 0;
            else
                halfMoveClock++;

            int fullMoveNumber = position.FullMoveNumber;
            if (position.SideToMove == Side.Black)
                fullMoveNumber++;

            return new Position(board, position.OpposingSide, enPassantSquare, castlingRights, halfMoveClock) { FullMoveNumber = fullMoveNumber };
        }

        private static CastlingRights UpdateCastlingRightsForBothSides(Board newBoard, CastlingRights castlingRights)
        {
            castlingRights = UpdateCastlingRightsForSide(castlingRights, newBoard, Side.White);
            castlingRights = UpdateCastlingRightsForSide(castlingRights, newBoard, Side.Black);

            return castlingRights;
        }

        private static CastlingRights UpdateCastlingRightsForSide(CastlingRights castlingRights, Board newBoard, Side side)
        {
            if ((Castling.KingSideRookFrom(side) & newBoard[side, Piece.Rook]) == 0)
            {
                castlingRights = castlingRights.RemoveKingSide(side);
            }

            if ((Castling.QueenSideRookFrom(side) & newBoard[side, Piece.Rook]) == 0)
            {
                castlingRights = castlingRights.RemoveQueenSide(side);
            }

            return castlingRights;
        }

        public static bool KingIsInCheck(Side side, Board board)
        {
            return board.IsAttacked(board[side, Piece.King], side.OpposingSide());
        }

        public static void ApplyMoveToBoard(Board board, Move move, out Piece fromPiece)
        {
            (Side fromSide, Piece fromPiece_) = board.GetPieceOnSquare(move.From);
            fromPiece = fromPiece_;
            Bitboard from = Bitboard.FromSquare(move.From);
            Bitboard to = Bitboard.FromSquare(move.To);
            Bitboard fromTo = from | to;

            if (move.Flags == MoveFlags.EnPassantCapture)
            {
                Bitboard capturedPawn = fromSide switch
                {
                    Side.White => to.SoutOne(),
                    Side.Black => to.NortOne(),
                    _ => throw new IndexOutOfRangeException(nameof(fromSide))
                };

                board[fromSide.OpposingSide(), Piece.Pawn] ^= capturedPawn;
            }
            else if (move.IsCapture)
            {
                (Side toSide, Piece toPiece) = board.GetPieceOnSquare(move.To);
                board[toSide, toPiece] ^= to;
            }
            else if (move.Flags == MoveFlags.KingCastle)
            {
                board[fromSide, Piece.Rook] ^= Castling.KingSideRookToFrom(fromSide);
            }
            else if (move.Flags == MoveFlags.QueenCastle)
            {
                board[fromSide, Piece.Rook] ^= Castling.QueenSideRookToFrom(fromSide);
            }

            board[fromSide, fromPiece] ^= fromTo;

            if (move.IsPromotion)
            {
                board[fromSide, fromPiece] ^= to;
                board[fromSide, move.PromotionPiece!.Value] ^= to;
            }
        }

        public static void UndoMoveOnBoard(Board board, Move move)
        {
            (Side fromSide, Piece fromPiece) = board.GetPieceOnSquare(move.To);
            Bitboard from = Bitboard.FromSquare(move.From);
            Bitboard to = Bitboard.FromSquare(move.To);
            Bitboard fromTo = from | to;

            if (move.IsPromotion)
            {
                board[fromSide, fromPiece] ^= to;
                board[fromSide, Piece.Pawn] ^= from;
            }
            else
            {
                board[fromSide, fromPiece] ^= fromTo;
            }

            if (move.Flags == MoveFlags.EnPassantCapture)
            {
                Bitboard capturedPawn = fromSide switch
                {
                    Side.White => to.SoutOne(),
                    Side.Black => to.NortOne(),
                    _ => throw new IndexOutOfRangeException(nameof(fromSide))
                };

                board[fromSide.OpposingSide(), Piece.Pawn] ^= capturedPawn;
            }
            else if (move.IsCapture)
            {
                (Side toSide, Piece toPiece) = (fromSide.OpposingSide(), move.CapturedPiece!.Value);
                board[toSide, toPiece] ^= to;
            }
            else if (move.Flags == MoveFlags.KingCastle)
            {
                board[fromSide, Piece.Rook] ^= Castling.KingSideRookToFrom(fromSide);
            }
            else if (move.Flags == MoveFlags.QueenCastle)
            {
                board[fromSide, Piece.Rook] ^= Castling.QueenSideRookToFrom(fromSide);
            }
        }
    }
}
