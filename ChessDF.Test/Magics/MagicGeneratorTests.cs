using ChessDF.Core;
using ChessDF.Magics;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test.Magics
{
    public class MagicGeneratorTests
    {
        [Theory]
        [InlineData(Square.b3, 0x00_02_02_02_02_7c_02_00)]
        [InlineData(Square.a1, 0x00_01_01_01_01_01_01_7e)]
        [InlineData(Square.h1, 0x00_80_80_80_80_80_80_7e)]
        [InlineData(Square.a8, 0x7e_01_01_01_01_01_01_00)]
        [InlineData(Square.h8, 0x7e_80_80_80_80_80_80_00)]
        [InlineData(Square.c1, 0x00_04_04_04_04_04_04_7a)]
        [InlineData(Square.a5, 0x00_01_01_7e_01_01_01_00)]
        [InlineData(Square.h7, 0x00_7e_80_80_80_80_80_00)]
        [InlineData(Square.e8, 0x6e_10_10_10_10_10_10_00)]
        public void GeneratesCorrectRookMasksForSquares(Square square, ulong expectedMask)
        {
            // Act
            Bitboard mask = MagicGenerator.RookMask(square);

            // Assert
            Assert.Equal<Bitboard>(expectedMask, mask);
        }

        [Theory]
        [InlineData(Square.d3, 0x00_00_40_22_14_00_14_00)]
        [InlineData(Square.a1, 0x00_40_20_10_08_04_02_00)]
        [InlineData(Square.h1, 0x00_02_04_08_10_20_40_00)]
        [InlineData(Square.a8, 0x00_02_04_08_10_20_40_00)]
        [InlineData(Square.h8, 0x00_40_20_10_08_04_02_00)]
        [InlineData(Square.c1, 0x00_00_00_40_20_10_0a_00)]
        [InlineData(Square.a5, 0x00_04_02_00_02_04_08_00)]
        [InlineData(Square.h7, 0x00_00_40_20_10_08_04_00)]
        [InlineData(Square.e8, 0x00_28_44_02_00_00_00_00)]
        public void GeneratesCorrectBishopMasksForSquares(Square square, ulong expectedMask)
        {
            // Act
            Bitboard mask = MagicGenerator.BishopMask(square);

            // Assert
            Assert.Equal<Bitboard>(expectedMask, mask);
        }

        [Fact]
        public void GetAllOccupanciesReturnsCorrectBitboards()
        {
            // Assemble
            Bitboard mask = 0x00_00_40_20_10_08_04_00; // Bishop on b1

            // Act
            Bitboard[] occupancies = MagicGenerator.GetAllPossibleOccupanciesForMask(mask);

            // Assert
            Bitboard[] expectedOccs = new Bitboard[]
            {
                0x00_00_00_00_00_00_00_00,
                0x00_00_00_00_00_00_04_00,
                0x00_00_00_00_00_08_00_00,
                0x00_00_00_00_00_08_04_00,
                0x00_00_00_00_10_00_00_00,
                0x00_00_00_00_10_00_04_00,
                0x00_00_00_00_10_08_00_00,
                0x00_00_00_00_10_08_04_00,
                0x00_00_00_20_00_00_00_00,
                0x00_00_00_20_00_00_04_00,
                0x00_00_00_20_00_08_00_00,
                0x00_00_00_20_00_08_04_00,
                0x00_00_00_20_10_00_00_00,
                0x00_00_00_20_10_00_04_00,
                0x00_00_00_20_10_08_00_00,
                0x00_00_00_20_10_08_04_00,
                0x00_00_40_00_00_00_00_00,
                0x00_00_40_00_00_00_04_00,
                0x00_00_40_00_00_08_00_00,
                0x00_00_40_00_00_08_04_00,
                0x00_00_40_00_10_00_00_00,
                0x00_00_40_00_10_00_04_00,
                0x00_00_40_00_10_08_00_00,
                0x00_00_40_00_10_08_04_00,
                0x00_00_40_20_00_00_00_00,
                0x00_00_40_20_00_00_04_00,
                0x00_00_40_20_00_08_00_00,
                0x00_00_40_20_00_08_04_00,
                0x00_00_40_20_10_00_00_00,
                0x00_00_40_20_10_00_04_00,
                0x00_00_40_20_10_08_00_00,
                0x00_00_40_20_10_08_04_00,
            };

            occupancies.Should().BeEquivalentTo(expectedOccs);
        }

        [Fact]
        public void CanFindAMagicNumberForASquare()
        {
            // Act
            (Bitboard magic, int bitCount) = MagicGenerator.FindMagic(Square.a1, 100_000_000, Piece.Rook);

            // Assert
            Assert.NotEqual<Bitboard>(0, magic);
            Assert.NotEqual(0, bitCount);
        }

        [Fact]
        public void CanLoadMagicsFromEmbeddedResource()
        {
            // Act
            var magics = MagicGenerator.LoadMagics();

            // Assert
            magics.Should().HaveCount(128);
        }
    }
}
