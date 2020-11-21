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
    public class SlidingMoveTests
    {
        [Theory]
        [InlineData(Square.a1, Direction.North, 0x01_01_01_01_01_01_01_00)]
        [InlineData(Square.a1, Direction.NorthEast, 0x80_40_20_10_08_04_02_00)]
        [InlineData(Square.a1, Direction.East, 0x00_00_00_00_00_00_00_fe)]
        [InlineData(Square.a1, Direction.South, 0)]
        [InlineData(Square.a1, Direction.West, 0)]
        [InlineData(Square.a1, Direction.SouthWest, 0)]
        [InlineData(Square.c4, Direction.NorthWest, 0x00_00_01_02_00_00_00_00)]
        [InlineData(Square.c4, Direction.North, 0x04_04_04_04_00_00_00_00)]
        [InlineData(Square.c4, Direction.SouthWest, 0x00_00_00_00_00_02_01_00)]
        [InlineData(Square.h5, Direction.West, 0x00_00_00_7f_00_00_00_00)]
        [InlineData(Square.f1, Direction.NorthEast, 0x00_00_00_00_00_80_40_00)]
        [InlineData(Square.e8, Direction.East, 0xe0_00_00_00_00_00_00_00)]
        [InlineData(Square.a8, Direction.South, 0x00_01_01_01_01_01_01_01)]
        [InlineData(Square.c6, Direction.SouthEast, 0x00_00_00_08_10_20_40_80)]
        public void CreatesCorrectRaysForGivenSquare(Square square, Direction direction, ulong expectedAttacks)
        {
            // Act
            var rayAttacks = SlidingPieceMoves.RayAttacks(square, direction);

            // Assert
            Assert.Equal<Bitboard>(expectedAttacks, rayAttacks);
        }

        [Theory]
        [InlineData(Square.d4, 0x08_08_08_08_f7_08_08_08)]
        [InlineData(Square.f2, 0x20_20_20_20_20_20_df_20)]
        [InlineData(Square.a8, 0xfe_01_01_01_01_01_01_01)]
        [InlineData(Square.g4, 0x40_40_40_40_bf_40_40_40)]
        [InlineData(Square.e6, 0x10_10_ef_10_10_10_10_10)]
        public void CreatesCorrectRookAttacksForSquare(Square square, ulong expectedAttacks)
        {
            // Act
            var rookAttacks = SlidingPieceMoves.RookAttacks(square);

            // Assert
            Assert.Equal<Bitboard>(expectedAttacks, rookAttacks);
        }

        [Theory]
        [InlineData(Square.a1, 0x80_40_20_10_08_04_02_00)]
        [InlineData(Square.a8, 0x00_02_04_08_10_20_40_80)]
        [InlineData(Square.h8, 0x00_40_20_10_08_04_02_01)]
        [InlineData(Square.h1, 0x01_02_04_08_10_20_40_00)]
        [InlineData(Square.a3, 0x20_10_08_04_02_00_02_04)]
        [InlineData(Square.e1, 0x00_00_00_01_82_44_28_00)]
        [InlineData(Square.h7, 0x40_00_40_20_10_08_04_02)]
        [InlineData(Square.c8, 0x00_0a_11_20_40_80_00_00)]
        [InlineData(Square.e4, 0x01_82_44_28_00_28_44_82)]
        public void CreatesCorrectBishopAttacksForSquare(Square square, ulong expectedAttacks)
        {
            // Act
            var bishopAttacks = SlidingPieceMoves.BishopAttacks(square);

            // Assert
            Assert.Equal<Bitboard>(expectedAttacks, bishopAttacks);
        }

        [Theory]
        [InlineData(Square.a1, 0x81_41_21_11_09_05_03_fe)]
        [InlineData(Square.a8, 0xfe_03_05_09_11_21_41_81)]
        [InlineData(Square.h8, 0x7f_c0_a0_90_88_84_82_81)]
        [InlineData(Square.h1, 0x81_82_84_88_90_a0_c0_7f)]
        [InlineData(Square.a3, 0x21_11_09_05_03_fe_03_05)]
        [InlineData(Square.e1, 0x10_10_10_11_92_54_38_ef)]
        [InlineData(Square.h7, 0xc0_7f_c0_a0_90_88_84_82)]
        [InlineData(Square.c8, 0xfb_0e_15_24_44_84_04_04)]
        [InlineData(Square.e4, 0x11_92_54_38_ef_38_54_92)]
        public void CreatesCorrectQueenAttacksForSquare(Square square, ulong expectedAttacks)
        {
            // Act
            var queenAttacks = SlidingPieceMoves.QueenAttacks(square);

            // Assert
            Assert.Equal<Bitboard>(expectedAttacks, queenAttacks);
        }

        [Theory]
        [InlineData(0xfd_fd_06_00_00_40_ff_df, Square.g2, 0x00_00_04_08_10_a0_00_a0)]
        [InlineData(0x49_ee_1a_0b_19_38_f2_69, Square.d3, 0x00_00_00_02_14_00_14_02)]
        [InlineData(0x49_ee_1a_0b_19_38_f2_69, Square.b2, 0x00_00_00_00_08_05_00_05)]
        [InlineData(0x49_ee_1a_0b_19_38_f2_69, Square.b7, 0x05_00_05_08_00_00_00_00)]
        [InlineData(0x49_ee_1a_0b_19_38_f2_69, Square.d6, 0x20_14_00_14_22_41_80_00)]
        public void CreatesCorrectBlockedBishopAttacksForSquare(ulong occupied, Square square, ulong expectedAttacks)
        {
            // Act
            var bishopAttacks = SlidingPieceMoves.BishopAttacks(occupied, square);

            // Assert
            Assert.Equal<Bitboard>(expectedAttacks, bishopAttacks);
        }

        [Theory]
        [InlineData(0x05_cf_af_24_d8_59_63_a2, Square.a7, 0x01_02_01_00_00_00_00_00)]
        [InlineData(0x05_cf_af_24_d8_59_63_a2, Square.c8, 0xfb_04_00_00_00_00_00_00)]
        [InlineData(0x05_cf_af_24_d8_59_63_a2, Square.f5, 0x00_00_20_dc_20_20_20_00)]
        [InlineData(0x05_cf_af_24_d8_59_63_a2, Square.f2, 0x00_00_00_20_20_20_5e_20)]
        public void CreatesCorrectBlockedRookAttacksForSquare(ulong occupied, Square square, ulong expectedAttacks)
        {
            // Act
            var rookAttacks = SlidingPieceMoves.RookAttacks(occupied, square);

            // Assert
            Assert.Equal<Bitboard>(expectedAttacks, rookAttacks);
        }

        [Theory]
        [InlineData(0x10_e5_b0_10_02_64_91_48, Square.a7, 0x03_06_03_05_09_11_21_40)]
        [InlineData(0x10_e5_b0_10_02_64_91_48, Square.b4, 0x22_12_0a_07_fd_07_02_02)]
        [InlineData(0xfd_e7_0c_10_14_00_ef_d7, Square.d8, 0x14_1c_28_40_80_00_00_00)]
        [InlineData(0xfd_e7_0c_10_14_00_ef_d7, Square.f7, 0x70_5c_70_a8_24_20_20_00)]
        public void CreatesCorrectBlockedQueenAttacksForSquare(ulong occupied, Square square, ulong expectedAttacks)
        {
            // Act
            var queenAttacks = SlidingPieceMoves.QueenAttacks(occupied, square);

            // Assert
            Assert.Equal<Bitboard>(expectedAttacks, queenAttacks);
        }
    }
}
