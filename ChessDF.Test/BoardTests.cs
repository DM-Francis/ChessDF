using ChessDF.Core;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test
{
    public class BoardTests
    {
        [Fact]
        public void CanCreateBoard()
        {
            // Assemble
            var board = new Board();

            var wp = board.WhitePawns;
        }

        [Fact]
        public void EqualityIsConsistent()
        {
            // Assemble
            var start1 = Board.StartingPosition;
            var start2 = Board.StartingPosition;

            // Act & Assert
            Assert.True(start1.Equals(start2));
            Assert.True(start1 == start2);
            Assert.Equal(start1.GetHashCode(), start2.GetHashCode());
        }

        [Fact]
        public void NonEqualityIsConsistent()
        {
            // Assemble
            var start1 = Board.StartingPosition;
            var start2 = new Board { WhitePawns = 0xFF00 };

            // Act & Assert
            Assert.False(start1.Equals(start2));
            Assert.False(start1 == start2);
            Assert.NotEqual(start1.GetHashCode(), start2.GetHashCode());
        }
    }
}
