using ChessDF.Core;
using ChessDF.Moves;
using System;
using Xunit;

namespace ChessDF.Test
{
    public class FENTests
    {
        [Fact]
        public void CanParseTheStartingPosition()
        {
            // Assemble
            string startingString = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            // Act
            var fen = new FEN(startingString);

            // Assert
            Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", fen.PiecePlacement);
            Assert.Equal(Side.White, fen.ActiveSide);
            Assert.Equal(CastlingRights.All, fen.CastlingAvailability);
            Assert.Null(fen.EnpassantTargetSquare);
            Assert.Equal(0, fen.HalfmoveClock);
            Assert.Equal(1, fen.FullmoveNumber);
        }
    }
}
