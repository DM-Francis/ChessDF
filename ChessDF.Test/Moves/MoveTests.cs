using ChessDF.Core;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test.Moves
{
    public class MoveTests
    {
        [Fact]
        public void PropertiesAreConsistentWithConstructor()
        {
            // Assemble
            Square from = Square.a6;
            Square to = Square.a5;
            MoveFlags flags = MoveFlags.QuietMove;

            // Act
            var move = new Move(from, to, flags);

            // Assert
            Assert.Equal(from, move.From);
            Assert.Equal(to, move.To);
            Assert.Equal(flags, move.Flags);
        }

        [Fact]
        public void CapturePropertyWorksWithStandardCaptures()
        {
            // Assemble
            Square from = Square.c3;
            Square to = Square.e5;
            var flags = MoveFlags.Capture;

            // Act
            var move = new Move(from, to, flags);

            // Assert
            Assert.True(move.IsCapture);
        }

        [Fact]
        public void CapturePropertyWorksWithPromotionCaptures()
        {
            // Assemble
            Square from = Square.d7;
            Square to = Square.d8;
            var flags = MoveFlags.Capture | MoveFlags.QueenPromotion;

            // Act
            var move = new Move(from, to, flags);

            // Assert
            Assert.True(move.IsCapture);
        }
    }
}
