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
    public class PawnMoveTests
    {
        [Fact]
        public void CreatesSinglePushTargetsCorrectly()
        {
            // Assemble
            Bitboard whitePawns = 0x00_00_00_08_00_01_F6_00;
            Bitboard blackPieces = 0x00_00_00_00_11_84_00_00;
            var board = new Board { WhitePawns = whitePawns, BlackQueens = blackPieces };

            // Act
            Bitboard singlePushTargets = board.SinglePawnPushTargets(Side.White);

            // Assert
            Assert.Equal<Bitboard>(0x00_00_08_00_00_72_00_00, singlePushTargets);
        }

        [Fact]
        public void CreatesDoublePushTargetsCorrectly()
        {
            // Assemble
            Bitboard whitePawns = 0x00_00_00_08_00_01_F6_00;
            Bitboard blackPieces = 0x00_00_00_00_11_84_00_00;
            var board = new Board { WhitePawns = whitePawns, BlackQueens = blackPieces };

            // Act
            Bitboard duoblePushTargets = board.DoublePawnPushTargets(Side.White);

            // Assert
            Assert.Equal<Bitboard>(0x00_00_00_00_62_00_00_00, duoblePushTargets);
        }
    }
}
