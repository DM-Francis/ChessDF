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
            Board newBoard = ApplyMoveToBoard(position.Board, move, out Piece fromPiece);

            if (KingIsInCheck(position.SideToMove, newBoard))
            {
                throw new IllegalMoveException("King is in check");
            }

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

            castlingRights = UpdateCastlingRightsForBothSides(newBoard, castlingRights);

            if (fromPiece == Piece.Pawn || move.IsCapture)
                halfMoveClock = 0;
            else
                halfMoveClock++;

            int fullMoveNumber = position.FullMoveNumber;
            if (position.SideToMove == Side.Black)
                fullMoveNumber++;

            return new Position(newBoard, position.OpposingSide, enPassantSquare, castlingRights, halfMoveClock) { FullMoveNumber = fullMoveNumber };
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
            Bitboard king = board[side, Piece.King];
            Bitboard enemyAttacks = board.AttacksBy(side.OpposingSide());

            return (king & enemyAttacks) != 0;
        }

        public static Board ApplyMoveToBoard(Board board, Move move, out Piece fromPiece)
        {
            (Side fromSide, Piece fromPiece_) = board.GetPieceOnSquare(move.From);
            fromPiece = fromPiece_;
            Bitboard from = Bitboard.FromSquare(move.From);
            Bitboard to = Bitboard.FromSquare(move.To);
            Bitboard fromTo = from | to;

            Board newBoard = board.Copy();
            newBoard[fromSide, fromPiece] ^= fromTo;

            if (move.IsPromotion)
            {
                newBoard[fromSide, fromPiece] ^= to;
                newBoard[fromSide, move.PromotionPiece!.Value] ^= to;
            }

            if (move.Flags == MoveFlags.EnPassantCapture)
            {
                Bitboard capturedPawn = fromSide switch
                {
                    Side.White => to.SoutOne(),
                    Side.Black => to.NortOne(),
                    _ => throw new IndexOutOfRangeException(nameof(fromSide))
                };

                newBoard[fromSide.OpposingSide(), Piece.Pawn] ^= capturedPawn;
            }
            else if (move.IsCapture)
            {
                (Side toSide, Piece toPiece) = board.GetPieceOnSquare(move.To);
                newBoard[toSide, toPiece] ^= to;
            }
            else if (move.Flags == MoveFlags.KingCastle)
            {
                newBoard[fromSide, Piece.Rook] ^= Castling.KingSideRookToFrom(fromSide);
            }
            else if (move.Flags == MoveFlags.QueenCastle)
            {
                newBoard[fromSide, Piece.Rook] ^= Castling.QueenSideRookToFrom(fromSide);
            }

            return newBoard;
        }
    }
}
