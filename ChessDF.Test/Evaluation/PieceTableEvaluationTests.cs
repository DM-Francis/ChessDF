using ChessDF.Core;
using ChessDF.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test.Evaluation
{
    public class PieceTableEvaluationTests
    {
        [Theory]
        [InlineData("8/8/8/8/8/8/PPPPPPPP/8 w - - 3 1 ", 8.1)]
        [InlineData("8/8/8/4P3/3P4/8/8/8 w - - 3 1", 2.45)]
        [InlineData("8/8/r7/8/8/8/4r1r1/8 b - - 3 1 ", 15.15)]
        public void EvaluatesCorrectScoreForPosition(string fen, double expectedScore)
        {
            var position = Position.FromFENString(fen);
            var evaluator = new PieceTableEvaluation();

            var score = evaluator.Evaluate(position);
            Assert.Equal(expectedScore, score);
        }
    }
}
