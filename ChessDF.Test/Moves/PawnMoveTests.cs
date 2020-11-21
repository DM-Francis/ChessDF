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
        public void CreatesWhiteSinglePushTargetsCorrectly()
        {
            // Assemble
            Bitboard whitePawns = 0x00_00_00_08_00_01_F6_00;
            Bitboard blackPieces = 0x00_00_00_00_11_84_00_00;
            Bitboard emptySquares = ~(whitePawns | blackPieces);

            // Act
            Bitboard singlePushTargets = PawnMoves.PawnSinglePushTargets(whitePawns, emptySquares, Side.White);

            // Assert
            Assert.Equal<Bitboard>(0x00_00_08_00_00_72_00_00, singlePushTargets);
        }

        [Fact]
        public void CreatesWhiteDoublePushTargetsCorrectly()
        {
            // Assemble
            Bitboard whitePawns = 0x00_00_00_08_00_01_F6_00;
            Bitboard blackPieces = 0x00_00_00_00_11_84_00_00;
            Bitboard emptySquares = ~(whitePawns | blackPieces);

            // Act
            Bitboard doublePushTargets = PawnMoves.PawnDoublePushTargets(whitePawns, emptySquares, Side.White);

            // Assert
            Assert.Equal<Bitboard>(0x00_00_00_00_62_00_00_00, doublePushTargets);
        }

        [Fact]
        public void CreatesBlackSinglePushTargetsCorrectly()
        {
            // Assemble
            Bitboard blackPawns = 0x00_c7_20_10_08_10_00_00;
            Bitboard whitePieces = 0x00_00_41_02_00_00_10_00;
            Bitboard emptySquares = ~(blackPawns | whitePieces);

            // Act
            Bitboard singlePushTargets = PawnMoves.PawnSinglePushTargets(blackPawns, emptySquares, Side.Black);

            // Assert
            Assert.Equal<Bitboard>(0x00_00_86_20_10_08_00_00, singlePushTargets);
        }

        [Fact]
        public void CreatesBlackDoublePushTargetsCorrectly()
        {
            // Assemble
            Bitboard blackPawns = 0x00_c7_20_10_08_10_00_00;
            Bitboard whitePieces = 0x00_00_41_02_00_00_10_00;
            Bitboard emptySquares = ~(blackPawns | whitePieces);

            // Act
            Bitboard doublePushTargets = PawnMoves.PawnDoublePushTargets(blackPawns, emptySquares, Side.Black);

            // Assert
            Assert.Equal<Bitboard>(0x00_00_00_84_00_00_00_00, doublePushTargets);
        }

        [Theory]
        [InlineData(Square.b4, 0x00_00_00_05_00_00_00_00)]
        [InlineData(Square.a7, 0x02_00_00_00_00_00_00_00)]
        [InlineData(Square.e2, 0x00_00_00_00_00_28_00_00)]
        [InlineData(Square.h5, 0x00_00_40_00_00_00_00_00)]
        public void GetsCorrectAttackSquaresForSingleWhitePawn(Square pawnSquare, ulong expectedAttackSquares)
        {
            // Act
            var attackSquares = PawnMoves.WhitePawnAttacks(pawnSquare);

            // Assert
            Assert.Equal<Bitboard>(expectedAttackSquares, attackSquares);
        }

        [Theory]
        [InlineData(Square.b4, 0x00_00_00_00_00_05_00_00)]
        [InlineData(Square.a7, 0x00_00_02_00_00_00_00_00)]
        [InlineData(Square.e2, 0x00_00_00_00_00_00_00_28)]
        [InlineData(Square.h5, 0x00_00_00_00_40_00_00_00)]
        public void GetsCorrectAttackSquaresForSingleBlackPawn(Square pawnSquare, ulong expectedAttackSquares)
        {
            // Act
            var attackSquares = PawnMoves.BlackPawnAttacks(pawnSquare);

            // Assert
            Assert.Equal<Bitboard>(expectedAttackSquares, attackSquares);
        }

        [Theory]
        [InlineData(Square.d1)]
        [InlineData(Square.e8)]
        public void ThrowsExceptionIfPawnIsOnFirstOrLastRank(Square pawnSquare)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => PawnMoves.WhitePawnAttacks(pawnSquare));
            Assert.Throws<ArgumentOutOfRangeException>(() => PawnMoves.BlackPawnAttacks(pawnSquare));
        }
    }
}
