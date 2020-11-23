using ChessDF.Core;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ChessDF.Test.Core
{
    public class BitBoardTests
    {
        [Theory]
        [InlineData(0x00_00_00_00_00_00_00_00, 0)]
        [InlineData(0x10_00_00_00_00_00_00_00, 1)]
        [InlineData(0x10_00_10_00_00_10_00_00, 3)]
        [InlineData(0xFF_FF_FF_FF_FF_FF_FF_FF, 64)]
        [InlineData(0xFF_FF_FF_FF_FF_FF_01_FF, 57)]
        public void PopCountIsAsExpected(ulong bits, int expectedCount)
        {
            // Assemble
            var board = new Bitboard(bits);

            // Act
            int count = board.PopCount();

            // Assert
            Assert.Equal(expectedCount, count);
        }

        [Theory]
        [InlineData(0xAB_00_12_45_00_0F_00_EE, 0xEE_00_0F_00_45_12_00_AB)]
        public void CanFlipVertically(ulong startBits, ulong flippedBits)
        {
            // Assemble
            var board = new Bitboard(startBits);

            // Act
            Bitboard flipped = board.FlipVertical();

            // Assert
            Assert.Equal(new Bitboard(flippedBits), flipped);
        }

        [Theory]
        [InlineData(0x1e_22_22_12_0e_0a_12_22, 0x78_44_44_48_70_50_48_44)]
        public void CanMirrorHorizontally(ulong startBits, ulong mirroredBits)
        {
            // Assemble
            var board = new Bitboard(startBits);

            // Act
            Bitboard mirrored = board.MirrorHorizontal();

            // Assert
            Assert.Equal(new Bitboard(mirroredBits), mirrored);
        }

        [Fact]
        public void CanSerializeBoard()
        {
            // Assemble
            var board = new Bitboard(0x00_00_00_00_00_00_01_01);

            // Act
            int[] bitIndices = board.Serialize();

            // Assert
            var expected = new int[] { 0, 8 };
            bitIndices.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void CanGetIndividualBits()
        {
            // Assemble
            var bb = new Bitboard(0x00_10_40_02_90_00_4a_00);

            // Act
            Bitboard[] singleBits = bb.IndividualBits();

            // Assert
            var expected = new Bitboard[]
            {
                0x00_00_00_00_00_00_02_00,
                0x00_00_00_00_00_00_08_00,
                0x00_00_00_00_00_00_40_00,
                0x00_00_00_00_10_00_00_00,
                0x00_00_00_00_80_00_00_00,
                0x00_00_00_02_00_00_00_00,
                0x00_00_40_00_00_00_00_00,
                0x00_10_00_00_00_00_00_00
            };

            singleBits.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(Square.a1, 0x00_00_00_00_00_00_00_01)]
        [InlineData(Square.h8, 0x80_00_00_00_00_00_00_00)]
        [InlineData(Square.d5, 0x00_00_00_08_00_00_00_00)]
        [InlineData(Square.h1, 0x00_00_00_00_00_00_00_80)]
        [InlineData(Square.a8, 0x01_00_00_00_00_00_00_00)]
        [InlineData(Square.b3, 0x00_00_00_00_00_02_00_00)]
        public void CanGenerateBitboardFromSquare(Square square, ulong expectedBitboard)
        {
            // Act
            var bb = Bitboard.FromSquare(square);

            // Assert
            Assert.Equal<Bitboard>(expectedBitboard, bb);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData(Square.a1, 0x00_00_00_00_00_00_00_01)]
        [InlineData(Square.h8, 0x80_00_00_00_00_00_00_00)]
        [InlineData(Square.d5, 0x00_00_00_08_00_00_00_00)]
        [InlineData(Square.h1, 0x00_00_00_00_00_00_00_80)]
        [InlineData(Square.a8, 0x01_00_00_00_00_00_00_00)]
        [InlineData(Square.b3, 0x00_00_00_00_00_02_00_00)]
        public void CanGenerateBitboardFromNullableSquare(Square? square, ulong expectedBitboard)
        {
            // Act
            var bb = Bitboard.FromSquare(square);

            // Assert
            Assert.Equal<Bitboard>(expectedBitboard, bb);
        }
    }
}
