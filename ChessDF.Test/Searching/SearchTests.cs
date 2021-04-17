using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Searching;
using FluentAssertions;
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
            var search = new NegaMaxSearch(new MateOnlyEvaluation());
            var position = Position.FromFENString("r1bqkb1r/pppp1ppp/2n2n2/4p2Q/2B1P3/8/PPPP1PPP/RNB1K1NR w KQkq - 0 1 ");

            // Act
            Move bestMove = search.Search(position, 1)[0].Move;

            // Assert
            var expectedMove = new Move(Square.h5, Square.f7, MoveFlags.Capture, Piece.Pawn);
            Assert.Equal(expectedMove, bestMove);
        }

        [Fact]
        public void CanSearchWithNegaMaxBasicScore()
        {
            // Assemble
            var search = new NegaMaxSearch(new BasicScoreEvaluation());
            var position = Position.FromFENString("2rr2k1/5Rp1/2b1p1Pp/1pq5/p4QN1/8/PPP5/1K3R2 b - - 0 1 ");

            // Act
            Move bestMove = search.Search(position, 2)[0].Move;

            // Assert
            var expectedMove = new Move(Square.a4, Square.a3, MoveFlags.QuietMove);
            Assert.Equal(expectedMove, bestMove);
        }

        [Fact]
        public void CanSearchWithAlphaBetaDepth5()
        {
            // Assemble
            var search = new AlphaBetaSearch(new BasicScoreEvaluation());
            var position = Position.FromFENString("2rr2k1/5Rp1/2b1p1Pp/1pq5/p4QN1/8/PPP5/1K3R2 b - - 0 1 ");

            // Act
            Move bestMove = search.Search(position, 5)[0].Move;

            // Assert
            var expectedMove = new Move(Square.c5, Square.c2, MoveFlags.Capture, Piece.Pawn);
            Assert.Equal(expectedMove, bestMove);
        }

        [Fact]
        public void AlphaBetaCanGetOutOfCheck()
        {
            // Assemble
            var search = new AlphaBetaSearch(new BasicScoreEvaluation());
            var position = Position.FromFENString("r1bqkb1r/2ppnppp/1p1Np3/p5N1/6PP/3PB1n1/PPP1PP2/R2QKB1R b KQk - 2 11 ");

            // Act
            Move bestMove = search.Search(position, 5)[0].Move;

            // Assert
            var expectedMove = new Move(Square.c7, Square.d6, MoveFlags.Capture, Piece.Knight);
            Assert.Equal(expectedMove, bestMove);
        }

        [Fact]
        public void CanFindCheckMates()
        {
            // Assemble
            var position = Position.FromFENString("8/7p/n3k2P/4n1p1/1p1K4/6q1/8/8 b - - 1 78 ");
            var search = new AlphaBetaSearch(new BasicScoreEvaluation());

            // Act
            var bestMoves = search.Search(position, 4);

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(Square.g3, Square.d3, MoveFlags.QuietMove),
                new Move(Square.g3, Square.f4, MoveFlags.QuietMove),
            };

            bestMoves.Select(b => b.Move).Should().BeEquivalentTo(expectedMoves);
        }
    }
}
