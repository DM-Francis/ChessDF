using ChessDF.Core;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test.Core
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

        [Fact]
        public void CanCreateBoardFromPositionString()
        {
            // Assemble
            string positionString = "rnbqkbnr/pp1ppppp/8/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R";

            // Act
            var board = Board.FromPiecePlacementString(positionString);

            // Assert
            var expectedBoard = new Board
            {
                WhitePawns = new Bitboard(0x00_00_00_00_10_00_ef_00),
                WhiteKnights = new Bitboard(0x00_00_00_00_00_20_00_02),
                WhiteBishops = new Bitboard(0x00_00_00_00_00_00_00_24),
                WhiteRooks = new Bitboard(0x00_00_00_00_00_00_00_81),
                WhiteQueens = new Bitboard(0x00_00_00_00_00_00_00_08),
                WhiteKing = new Bitboard(0x00_00_00_00_00_00_00_10),

                BlackPawns = new Bitboard(0x00_fb_00_04_00_00_00_00),
                BlackKnights = new Bitboard(0x42_00_00_00_00_00_00_00),
                BlackBishops = new Bitboard(0x24_00_00_00_00_00_00_00),
                BlackRooks = new Bitboard(0x81_00_00_00_00_00_00_00),
                BlackQueens = new Bitboard(0x08_00_00_00_00_00_00_00),
                BlackKing = new Bitboard(0x10_00_00_00_00_00_00_00)
            };

            Assert.Equal(expectedBoard, board);
        }

        [Fact]
        public void CanCreateBoardFromPositionStringKiwiPete()
        {
            // Assemble
            string positionString = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R";

            // Act
            var board = Board.FromPiecePlacementString(positionString);

            // Assert
            var expectedBoard = new Board
            {
                WhitePawns = 0x00_00_00_08_10_00_e7_00,
                WhiteKnights = 0x00_00_00_10_00_04_00_00,
                WhiteBishops = 0x00_00_00_00_00_00_18_00,
                WhiteRooks = 0x00_00_00_00_00_00_00_81,
                WhiteQueens = 0x00_00_00_00_00_20_00_00,
                WhiteKing = 0x00_00_00_00_00_00_00_10,

                BlackPawns = 0x00_2d_50_00_02_80_00_00,
                BlackKnights = 0x00_00_22_00_00_00_00_00,
                BlackBishops = 0x00_40_01_00_00_00_00_00,
                BlackRooks = 0x81_00_00_00_00_00_00_00,
                BlackQueens = 0x00_10_00_00_00_00_00_00,
                BlackKing = 0x10_00_00_00_00_00_00_00
            };

            Assert.Equal(expectedBoard, board);
        }

        [Fact]
        public void CanConvertStartingBoardToPiecePlacementString()
        {
            // Assemble
            var board = Board.StartingPosition;

            // Act
            string placementstring = board.ToPiecePlacementString();

            // Assert
            Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", placementstring);
        }

        [Fact]
        public void ToPlacementStringIsConsistentWithFromPlacementString()
        {
            // Assemble
            string initialPlacementString = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R";
            var board = Board.FromPiecePlacementString(initialPlacementString);

            // Act
            string placementstring = board.ToPiecePlacementString();

            // Assert
            Assert.Equal(initialPlacementString, placementstring);
        }
    }
}
