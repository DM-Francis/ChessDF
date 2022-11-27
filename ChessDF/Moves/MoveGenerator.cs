using ChessDF.Core;
using ChessDF.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ChessDF.Moves
{
    public class MoveGenerator
    {
        public static List<Move> GetAllMoves(Position position, bool onlyLegal = true)
        {
            var allMoves = new List<Move>();

            AddAllPawnMoves(position, allMoves);
            AddKnightMoves(position, allMoves);
            AddBishopMoves(position, allMoves);
            AddRookMoves(position, allMoves);
            AddQueenMoves(position, allMoves);
            AddKingMoves(position, allMoves);
            AddCastlingMoves(position, allMoves);

            if (onlyLegal)
            {
                var legalMoves = allMoves.Where(m => !MoveIsIllegal(m, position.Board, position.SideToMove)).ToList();
                return legalMoves;
            }

            return allMoves;
        }

        internal static void AddAllPawnMoves(Position position, List<Move> allMoves)
        {
            Board board = position.Board;
            Side sideToMove = position.SideToMove;
            Bitboard enPassantTarget = Bitboard.FromSquare(position.EnPassantSquare);
            Bitboard pawns = board[sideToMove, Piece.Pawn];

            // Single pushes
            Bitboard singlePawnPushTargets = PawnMoves.PawnSinglePushTargets(pawns, board.EmptySquares, sideToMove);
            Bitboard singlePawnPushSources = sideToMove == Side.White ? singlePawnPushTargets.SoutOne() : singlePawnPushTargets.NortOne();
            AddPawnPushes(singlePawnPushTargets, singlePawnPushSources, allMoves, false);

            // Double pushes
            Bitboard doublePawnPushTargets = PawnMoves.PawnDoublePushTargets(pawns, board.EmptySquares, sideToMove);
            Bitboard doublePawnPushSources = sideToMove == Side.White ? doublePawnPushTargets.SoutOne().SoutOne() : doublePawnPushTargets.NortOne().NortOne();
            AddPawnPushes(doublePawnPushTargets, doublePawnPushSources, allMoves, true);

            // Pawn captures
            var pawnSquares = pawns.Serialize();
            for (int p = 0; p < pawnSquares.Length; p++)
            {
                Square from = (Square)pawnSquares[p];
                Bitboard validAttacks;
                if (sideToMove == Side.White)
                    validAttacks = PawnMoves.WhitePawnAttacks(from) & (board.BlackPieces | enPassantTarget);
                else if (sideToMove == Side.Black)
                    validAttacks = PawnMoves.BlackPawnAttacks(from) & (board.WhitePieces | enPassantTarget);
                else
                    throw new IndexOutOfRangeException(nameof(sideToMove));

                var targetSquares = validAttacks.Serialize();
                for (int t = 0; t < targetSquares.Length; t++)
                {
                    Square to = (Square)targetSquares[t];

                    if (to == position.EnPassantSquare)
                    {
                        allMoves.Add(new Move(from, to, MoveFlags.EnPassantCapture, Piece.Pawn));
                        continue;
                    }

                    (_, Piece targetPiece) = board.GetPieceOnSquare(to);
                    if (IsFinalRank(to))
                    {
                        var promotions = new[]
                        {
                            new Move(from, to, MoveFlags.Capture | MoveFlags.KnightPromotion, targetPiece),
                            new Move(from, to, MoveFlags.Capture | MoveFlags.BishopPromotion, targetPiece),
                            new Move(from, to, MoveFlags.Capture | MoveFlags.RookPromotion, targetPiece),
                            new Move(from, to, MoveFlags.Capture | MoveFlags.QueenPromotion, targetPiece),
                        };
                        allMoves.AddRange(promotions);
                    }
                    else
                    {
                        var flags = to == position.EnPassantSquare ? MoveFlags.EnPassantCapture : MoveFlags.Capture;
                        allMoves.Add(new Move(from, to, flags, targetPiece));
                    }
                }
            }
        }

        private static void AddPawnPushes(Bitboard pawnTargets, Bitboard pawnSources, List<Move> allMoves, bool isDoublePush)
        {
            var pushSourceSq = pawnSources.Serialize();
            var pushTargetSq = pawnTargets.Serialize();

            for (int i = 0; i < pushSourceSq.Length; i++)
            {
                Square from = (Square)pushSourceSq[i];
                Square to = (Square)pushTargetSq[i];

                if (IsFinalRank(to))
                {
                    var promotions = new[]
                    {
                        new Move(from, to, MoveFlags.KnightPromotion),
                        new Move(from, to, MoveFlags.BishopPromotion),
                        new Move(from, to, MoveFlags.RookPromotion),
                        new Move(from, to, MoveFlags.QueenPromotion),
                    };
                    allMoves.AddRange(promotions);
                }
                else
                {
                    MoveFlags flags = isDoublePush ? MoveFlags.DoublePawnPush : MoveFlags.QuietMove;
                    allMoves.Add(new Move(from, to, flags));
                }
            }
        }

        internal static void AddKnightMoves(Position position, List<Move> allMoves)
        {
            Board board = position.Board;
            Side sideToMove = position.SideToMove;
            Bitboard knights = board[sideToMove, Piece.Knight];

            var knightSources = knights.Serialize();
            for (int i = 0; i < knightSources.Length; i++)
            {
                Square from = (Square)knightSources[i];
                Bitboard attacks = KnightMoves.KnightAttacks(from);
                AddMovesFromAttacks(attacks, from, sideToMove, board, allMoves);
            }
        }

        internal static void AddBishopMoves(Position position, List<Move> allMoves)
        {
            Board board = position.Board;
            Side sideToMove = position.SideToMove;
            Bitboard bishops = board[sideToMove, Piece.Bishop];

            var bishopSources = bishops.Serialize();
            for (int i = 0; i < bishopSources.Length; i++)
            {
                Square from = (Square)bishopSources[i];
                Bitboard attacks = SlidingPieceMoves.BishopAttacks(from, board.OccupiedSquares);
                AddMovesFromAttacks(attacks, from, sideToMove, board, allMoves);
            }
        }

        internal static void AddRookMoves(Position position, List<Move> allMoves)
        {
            Board board = position.Board;
            Side sideToMove = position.SideToMove;
            Bitboard rooks = board[sideToMove, Piece.Rook];

            var rookSources = rooks.Serialize();
            for (int i = 0; i < rookSources.Length; i++)
            {
                Square from = (Square)rookSources[i];
                Bitboard attacks = SlidingPieceMoves.RookAttacks(from, board.OccupiedSquares);
                AddMovesFromAttacks(attacks, from, sideToMove, board, allMoves);
            }
        }

        internal static void AddQueenMoves(Position position, List<Move> allMoves)
        {
            Board board = position.Board;
            Side sideToMove = position.SideToMove;
            Bitboard queens = board[sideToMove, Piece.Queen];

            var queenSources = queens.Serialize();
            for (int i = 0; i < queenSources.Length; i++)
            {
                Square from = (Square)queenSources[i];
                Bitboard attacks = SlidingPieceMoves.QueenAttacks(from, board.OccupiedSquares);
                AddMovesFromAttacks(attacks, from, sideToMove, board, allMoves);
            }
        }

        internal static void AddKingMoves(Position position, List<Move> allMoves)
        {
            Board board = position.Board;
            Side sideToMove = position.SideToMove;
            Bitboard king = board[sideToMove, Piece.King];

            var kings = king.Serialize();
            if (kings.Length == 0)
                return;

            int kingSource = kings[0];
            Square from = (Square)kingSource;
            Bitboard attacks = KingMoves.KingAttacks(from);
            AddMovesFromAttacks(attacks, from, sideToMove, board, allMoves);
        }

        internal static void AddCastlingMoves(Position position, List<Move> allMoves)
        {
            Side side = position.SideToMove;
            Board board = position.Board;

            if (board[side, Piece.King] == 0)
                return;

            if (Castling.CanCastleKingside(position.CastlingRights, side))
            {
                var kingsideBetween = Castling.KingSideBetween(side);
                bool spaceAvailable = BitUtils.IsASubsetOfB(kingsideBetween, board.EmptySquares);

                var kingsideChecks = Castling.KingSideChecks(side);
                Bitboard enemyAttacks = board.AttacksBy(position.OpposingSide);
                bool avoidsChecks = (kingsideChecks & enemyAttacks) == 0;

                if (spaceAvailable && avoidsChecks)
                {
                    Bitboard king = board[side, Piece.King];
                    Bitboard castleTarget = king.EastOne().EastOne();
                    Square from = (Square)king.FirstIndex();
                    Square to = (Square)castleTarget.FirstIndex();

                    allMoves.Add(new Move(from, to, MoveFlags.KingCastle));
                }
            }

            if (Castling.CanCastleQueenside(position.CastlingRights, side))
            {
                var queensideBetween = Castling.QueenSideBetween(side);
                bool spaceAvailable = BitUtils.IsASubsetOfB(queensideBetween, board.EmptySquares);

                var queensideChecks = Castling.QueenSideChecks(side);
                Bitboard enemyAttacks = board.AttacksBy(position.OpposingSide);
                bool avoidsChecks = (queensideChecks & enemyAttacks) == 0;

                if (spaceAvailable && avoidsChecks)
                {
                    Bitboard king = board[side, Piece.King];
                    Bitboard castleTarget = king.WestOne().WestOne();
                    Square from = (Square)king.FirstIndex();
                    Square to = (Square)castleTarget.FirstIndex();

                    allMoves.Add(new Move(from, to, MoveFlags.QueenCastle));
                }
            }
        }

        private static void AddMovesFromAttacks(Bitboard attacks, Square from, Side sideToMove, Board board, List<Move> allMoves)
        {
            Bitboard validAttacks = attacks & ~board.FriendlyPieces(sideToMove);

            var targetSquares = validAttacks.Serialize();
            for (int t = 0; t < targetSquares.Length; t++)
            {
                Square to = (Square)targetSquares[t];
                bool isCapture = (Bitboard.FromSquare(to) & board.OpposingPieces(sideToMove)) != 0;

                if (isCapture)
                {
                    (_, Piece targetPiece) = board.GetPieceOnSquare(to);
                    allMoves.Add(new Move(from, to, MoveFlags.Capture, targetPiece));
                }
                else
                {
                    allMoves.Add(new Move(from, to, MoveFlags.QuietMove));
                }
            }
        }

        private static bool IsFinalRank(Square square) => (int)square < 8 || (int)square >= 56;

        public static bool MoveIsIllegal(Move move, Board board, Side side)
        {
            Mover.ApplyMoveToBoard(board, move, out _);
            bool illegal = Mover.KingIsInCheck(side, board);
            Mover.UndoMoveOnBoard(board, move);
            return illegal;
        }
    }
}
