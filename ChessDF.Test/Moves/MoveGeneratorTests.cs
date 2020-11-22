using ChessDF.Core;
using ChessDF.Moves;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test.Moves
{
    public class MoveGeneratorTests
    {
        [Fact]
        public void CanGeneratePawnMovesForBasicPosition()
        {
            // Assemble
            var generator = new MoveGenerator();
            var board = new Board
            {
                WhitePawns = 0x00_00_00_00_00_00_ff_00,
                BlackPawns = 0x00_00_00_00_00_14_00_00
            };

            // Act
            var moves = generator.GenerateAllMovesForPosition(board, Side.White);

            // Assert
            moves.Should().HaveCount(16);
            moves.Where(m => (m.Flags & MoveFlags.Capture) != 0).Should().HaveCount(4);
        }

        [Fact]
        public void CanGeneratePawnMovesForBasicPositionWithPromotions()
        {
            // Assemble
            var generator = new MoveGenerator();
            var board = new Board
            {
                WhitePawns = 0x00_12_20_00_00_00_00_00,
                BlackQueens = 0x08_00_00_00_00_00_00_00
            };

            // Act
            var moves = generator.GenerateAllMovesForPosition(board, Side.White);

            // Assert
            moves.Should().HaveCount(13);
            moves.Where(m => (m.Flags & MoveFlags.Capture) != 0).Should().HaveCount(4);
            moves.Where(m => (m.Flags & MoveFlags.KnightPromotion) != 0).Should().HaveCount(12); // All promotions
        }
    }
}
