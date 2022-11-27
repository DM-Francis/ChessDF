using ChessDF.Core;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Evaluation
{
    class ScoreWithMobilityEvaluation : IEvaluator
    {
        public double Evaluate(Position position)
        {
            double score = (position.Board.WhiteKing.PopCount() - position.Board.BlackKing.PopCount()) * 200
                + (position.Board.WhiteQueens.PopCount() - position.Board.BlackQueens.PopCount()) * 9
                + (position.Board.WhiteRooks.PopCount() - position.Board.BlackRooks.PopCount()) * 5
                + (position.Board.WhiteBishops.PopCount() - position.Board.BlackBishops.PopCount()) * 3
                + (position.Board.WhiteKnights.PopCount() - position.Board.BlackKnights.PopCount()) * 3
                + (position.Board.WhitePawns.PopCount() - position.Board.BlackPawns.PopCount()) * 1;

            int totalMovesAvailable = position.GetAllLegalMoves().Count;
            int enemyMovesAvailable = MoveGenerator.GetAllMoves(position.WithOpposingSideToMove(), onlyLegal: true).Count;

            score += 0.1 * (totalMovesAvailable - enemyMovesAvailable);

            if (position.SideToMove == Side.White)
                return score;
            else
                return -1 * score;
        }
    }
}
