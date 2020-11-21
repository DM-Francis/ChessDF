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
    public class KingMoveTests
    {
        [Theory]
        [InlineData(Square.a1, 0x00_00_00_00_00_00_03_02)]
        [InlineData(Square.d1, 0x00_00_00_00_00_00_1c_14)]
        [InlineData(Square.c4, 0x00_00_00_0e_0a_0e_00_00)]
        [InlineData(Square.d8, 0x14_1c_00_00_00_00_00_00)]
        [InlineData(Square.h6, 0x00_c0_40_c0_00_00_00_00)]
        public void CanCreateCorrectKingAttacksForGivenSquare(Square square, ulong expectedAttacks)
        {
            // Act
            var attacks = KingMoves.KingAttacks(square);

            // Assert
            Assert.Equal<Bitboard>(expectedAttacks, attacks);
        }
    }
}
