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
    public class KnightMoveTests
    {
        [Theory]
        [InlineData(Square.d5, 0x00_14_22_00_22_14_00_00)]
        [InlineData(Square.b6, 0x05_08_00_08_05_00_00_00)]
        [InlineData(Square.a1, 0x00_00_00_00_00_02_04_00)]
        [InlineData(Square.e1, 0x00_00_00_00_00_28_44_00)]
        [InlineData(Square.h5, 0x00_40_20_00_20_40_00_00)]
        [InlineData(Square.f8, 0x00_88_50_00_00_00_00_00)]
        public void CreatesCorrectKnightAttacksForGivenSquare(Square square, ulong expectedAttacks)
        {
            // Act
            Bitboard attacks = KnightMoves.KnightAttacks(square);

            // Assert
            Assert.Equal<Bitboard>(expectedAttacks, attacks);
        }
    }
}
