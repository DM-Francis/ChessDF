using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    public class MoveGenerator
    {
        public List<Move> GenerateAllMovesForPosition(Position position)
        {
            var allMoves = new List<Move>();

            AddAllPawnMoves(position, allMoves);
            AddKnightMoves(position, allMoves);

            return allMoves;
        }

        private static void AddAllPawnMoves(Position position, List<Move> allMoves)
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
            int[] pawnSquares = pawns.Serialize();
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

                int[] targetSquares = validAttacks.Serialize();
                for (int t = 0; t < targetSquares.Length; t++)
                {
                    Square to = (Square)targetSquares[t];
                    if (IsFinalRank(to))
                    {
                        var promotions = new[]
                        {
                            new Move(from, to, MoveFlags.Capture | MoveFlags.KnightPromotion),
                            new Move(from, to, MoveFlags.Capture | MoveFlags.BishopPromotion),
                            new Move(from, to, MoveFlags.Capture | MoveFlags.RookPromotion),
                            new Move(from, to, MoveFlags.Capture | MoveFlags.QueenPromotion),
                        };
                        allMoves.AddRange(promotions);
                    }
                    else
                    {
                        var flags = to == position.EnPassantSquare ? MoveFlags.EnPassantCapture : MoveFlags.Capture;
                        allMoves.Add(new Move(from, to, flags));
                    }
                }
            }
        }

        private static void AddPawnPushes(Bitboard pawnTargets, Bitboard pawnSources, List<Move> allMoves, bool isDoublePush)
        {
            int[] pushSourceSq = pawnSources.Serialize();
            int[] pushTargetSq = pawnTargets.Serialize();

            for (int i = 0; i < pushSourceSq.Length; i++)
            {
                Square from = (Square)pushSourceSq[i];
                Square to = (Square)pushTargetSq[i];

                if (IsFinalRank(to))
                {
                    var promotions = new []
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

        private static void AddKnightMoves(Position position, List<Move> allMoves)
        {
            Board board = position.Board;
            Side sideToMove = position.SideToMove;
            Bitboard knights = board[sideToMove, Piece.Knight];

            int[] knightSources = knights.Serialize();
            for (int i = 0; i < knightSources.Length; i++)
            {
                Square from = (Square)knightSources[i];
                Bitboard attacks = KnightMoves.KnightAttacks(from);
                Bitboard validAttacks = attacks & ~board.FriendlyPieces(sideToMove);

                int[] targetSquares = validAttacks.Serialize();
                for (int t = 0; t < targetSquares.Length; t++)
                {
                    Square to = (Square)targetSquares[t];
                    bool isCapture = (Bitboard.FromSquare(to) & board.OpposingPieces(sideToMove)) != 0;
                    var flags = isCapture ? MoveFlags.Capture : MoveFlags.QuietMove;
                    allMoves.Add(new Move(from, to, flags));                    
                }
            }
        }


        private static bool IsFinalRank(Square square) => (int)square < 8 || (int)square >= 56;
    }
}
