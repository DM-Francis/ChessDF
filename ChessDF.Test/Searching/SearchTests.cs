using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test.Searching
{
    public class SearchTests
    {
        [Fact]
        public void CanSearchWithNegaMax()
        {
            // Assemble
            var search = new Search(new MateOnlyEvaluation());
            var position = Position.FromFENString("r1bqkb1r/pppp1ppp/2n2n2/4p2Q/2B1P3/8/PPPP1PPP/RNB1K1NR w KQkq - 0 1 ");

            // Act
            Move bestMove = search.RootNegaMax(position, 1);

            // Assert
            var expectedMove = new Move(Square.h5, Square.f7, MoveFlags.Capture);
            Assert.Equal(expectedMove, bestMove);
        }

        [Fact]
        public void CanSearchWithNegaMaxBasicScore()
        {
            // Assemble
            var search = new Search(new BasicScoreEvaluation());
            var position = Position.FromFENString("2rr2k1/5Rp1/2b1p1Pp/1pq5/p4QN1/8/PPP5/1K3R2 b - - 0 1 ");

            // Act
            Move bestMove = search.RootNegaMax(position, 3);

            // Assert
            var expectedMove = new Move(Square.c5, Square.c2, MoveFlags.Capture);
            Assert.Equal(expectedMove, bestMove);
        }
    }
}
