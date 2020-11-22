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
        public List<Move> GenerateAllMovesForPosition(Board board, Side sideToPlay)
        {
            var allMoves = new List<Move>();

            // Generate pawn moves
            Bitboard pawns = board[sideToPlay, Piece.Pawn];

            // Single pushes
            Bitboard singlePawnPushTargets = PawnMoves.PawnSinglePushTargets(pawns, board.EmptySquares, sideToPlay);
            Bitboard singlePawnPushSources = sideToPlay == Side.White ? singlePawnPushTargets.SoutOne() : singlePawnPushTargets.NortOne();
            AddPawnMoves(singlePawnPushTargets, singlePawnPushSources, allMoves, false);

            // Double pushes
            Bitboard doublePawnPushTargets = PawnMoves.PawnDoublePushTargets(pawns, board.EmptySquares, sideToPlay);
            Bitboard doublePawnPushSources = sideToPlay == Side.White ? doublePawnPushTargets.SoutOne().SoutOne() : doublePawnPushTargets.NortOne().NortOne();
            AddPawnMoves(doublePawnPushTargets, doublePawnPushSources, allMoves, true);

            // Pawn captures
            int[] pawnSquares = pawns.Serialize();
            for (int p = 0; p < pawnSquares.Length; p++)
            {
                Square from = (Square)pawnSquares[p];
                Bitboard validAttacks;
                if (sideToPlay == Side.White)
                    validAttacks = PawnMoves.WhitePawnAttacks(from) & board.BlackPieces;
                else if (sideToPlay == Side.Black)
                    validAttacks = PawnMoves.BlackPawnAttacks(from) & board.WhitePieces;
                else
                    throw new ArgumentOutOfRangeException(nameof(sideToPlay));

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
                        allMoves.Add(new Move(from, to, MoveFlags.Capture));
                    }
                }
            }

            return allMoves;
        }

        private static void AddPawnMoves(Bitboard pawnTargets, Bitboard pawnSources, List<Move> allMoves, bool isDoublePush)
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

        private static bool IsFinalRank(Square square) => (int)square < 8 || (int)square >= 56;
    }
}
