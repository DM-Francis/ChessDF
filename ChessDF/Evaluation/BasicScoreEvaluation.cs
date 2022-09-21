using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Evaluation
{
    public class BasicScoreEvaluation : IEvaluator
    {
        public double Evaluate(Position position)
        {
            double score = (position.Board.WhiteKing.PopCount() - position.Board.BlackKing.PopCount()) * 200
                + (position.Board.WhiteQueens.PopCount() - position.Board.BlackQueens.PopCount()) * 9
                + (position.Board.WhiteRooks.PopCount() - position.Board.BlackRooks.PopCount()) * 5
                + (position.Board.WhiteBishops.PopCount() - position.Board.BlackBishops.PopCount()) * 3.3
                + (position.Board.WhiteKnights.PopCount() - position.Board.BlackKnights.PopCount()) * 3.2
                + (position.Board.WhitePawns.PopCount() - position.Board.BlackPawns.PopCount()) * 1;

            if (position.SideToMove == Side.White)
                return score;
            else
                return -1 * score;
        }
    }

    public static class BasicScoreExtensions
    {
        public static double ScoreValue(this Piece piece)
        {
            return piece switch
            {
                Piece.Pawn => 1,
                Piece.Knight => 3.2,
                Piece.Bishop => 3.3,
                Piece.Rook => 5,
                Piece.Queen => 9,
                Piece.King => 200,
                _ => throw new ArgumentOutOfRangeException(nameof(piece), "Unrecognised piece type")
            };
        }
    }
}
