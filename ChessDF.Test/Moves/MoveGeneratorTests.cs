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

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);

            // Act
            var moves = generator.GenerateAllMovesForPosition(position);

            // Assert
            moves.Should().HaveCount(16);
            moves.Where(m => (m.Flags & MoveFlags.Capture) != 0).Should().HaveCount(4);
        }

        [Fact]
        public void CanGeneratePawnMovesForPositionWhite()
        {
            // Assemble
            var generator = new MoveGenerator();
            var board = new Board
            {
                WhitePawns = 0x00_00_00_01_04_0a_f0_00,
                BlackPawns = 0x00_00_1d_82_40_20_00_00
            };

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);

            // Act
            var moves = generator.GenerateAllMovesForPosition(position);

            // Assert
            moves.Should().HaveCount(11);
            moves.Where(m => (m.Flags & MoveFlags.Capture) != 0).Should().HaveCount(3);
        }

        [Fact]
        public void CanGeneratePawnMovesForPositionBlack()
        {
            // Assemble
            var generator = new MoveGenerator();
            var board = new Board
            {
                WhitePawns = 0x00_00_00_01_04_0a_f0_00,
                BlackPawns = 0x00_00_1d_82_40_20_00_00
            };

            var position = new Position(board, Side.Black, null, CastlingRights.None, 0);

            // Act
            var moves = generator.GenerateAllMovesForPosition(position);

            // Assert
            moves.Should().HaveCount(9);
            moves.Where(m => (m.Flags & MoveFlags.Capture) != 0).Should().HaveCount(3);
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

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);

            // Act
            var moves = generator.GenerateAllMovesForPosition(position);

            // Assert
            moves.Should().HaveCount(13);
            moves.Where(m => (m.Flags & MoveFlags.Capture) != 0).Should().HaveCount(4);
            moves.Where(m => (m.Flags & MoveFlags.KnightPromotion) != 0).Should().HaveCount(12); // All promotions
        }

        [Fact]
        public void CanGetMovesForPositionWithEnpassantWhite()
        {
            // Assemble
            var generator = new MoveGenerator();
            var board = new Board
            {
                WhitePawns = 0x00_00_00_04_00_00_00_00,
                BlackPawns = 0x00_00_00_08_00_00_00_00
            };

            var position = new Position(board, Side.White, Square.d6, CastlingRights.None, 0);

            // Act
            var moves = generator.GenerateAllMovesForPosition(position);

            // Assert
            moves.Should().HaveCount(2);
            moves.Where(m => m.Flags == MoveFlags.EnPassantCapture).Should().HaveCount(1);
        }

        [Fact]
        public void CanGetMovesForPositionWithEnpassantBlack()
        {
            // Assemble
            var generator = new MoveGenerator();
            var board = new Board
            {
                WhitePawns = 0x00_00_00_00_01_00_00_00,
                BlackPawns = 0x00_00_00_00_02_00_00_00
            };

            var position = new Position(board, Side.Black, Square.a3, CastlingRights.None, 0);

            // Act
            var moves = generator.GenerateAllMovesForPosition(position);

            // Assert
            moves.Should().HaveCount(2);
            moves.Where(m => m.Flags == MoveFlags.EnPassantCapture).Should().HaveCount(1);
        }

        [Fact]
        public void CanGetMovesForPositionWith2EnpassantWhite()
        {
            // Assemble
            var generator = new MoveGenerator();
            var board = new Board
            {
                WhitePawns = 0x00_00_00_14_00_00_00_00,
                BlackPawns = 0x00_00_00_08_00_00_00_00
            };

            var position = new Position(board, Side.White, Square.d6, CastlingRights.None, 0);

            // Act
            var moves = generator.GenerateAllMovesForPosition(position);

            // Assert
            moves.Should().HaveCount(4);
            moves.Where(m => m.Flags == MoveFlags.EnPassantCapture).Should().HaveCount(2);
        }
    }
}
