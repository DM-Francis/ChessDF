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
            var board = new Board
            {
                WhitePawns = 0x00_00_00_00_00_00_ff_00,
                BlackPawns = 0x00_00_00_00_00_14_00_00
            };

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            moves.Should().HaveCount(16);
            moves.Where(m => (m.Flags & MoveFlags.Capture) != 0).Should().HaveCount(4);
        }

        [Fact]
        public void CanGeneratePawnMovesForPositionWhite()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_01_04_0a_f0_00,
                BlackPawns = 0x00_00_1d_82_40_20_00_00
            };

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            moves.Should().HaveCount(11);
            moves.Where(m => (m.Flags & MoveFlags.Capture) != 0).Should().HaveCount(3);
        }

        [Fact]
        public void CanGeneratePawnMovesForPositionBlack()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_01_04_0a_f0_00,
                BlackPawns = 0x00_00_1d_82_40_20_00_00
            };

            var position = new Position(board, Side.Black, null, CastlingRights.None, 0);

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            moves.Should().HaveCount(9);
            moves.Where(m => (m.Flags & MoveFlags.Capture) != 0).Should().HaveCount(3);
        }

        [Fact]
        public void CanGeneratePawnMovesForBasicPositionWithPromotions()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_12_20_00_00_00_00_00,
                BlackQueens = 0x08_00_00_00_00_00_00_00
            };

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            moves.Should().HaveCount(13);
            moves.Where(m => m.IsCapture).Should().HaveCount(4);
            moves.Where(m => m.IsPromotion).Should().HaveCount(12);
        }

        [Fact]
        public void CanGetMovesForPositionWithEnpassantWhite()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_04_00_00_00_00,
                BlackPawns = 0x00_00_00_08_00_00_00_00
            };

            var position = new Position(board, Side.White, Square.d6, CastlingRights.None, 0);

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            moves.Should().HaveCount(2);
            moves.Where(m => m.Flags == MoveFlags.EnPassantCapture).Should().HaveCount(1);
        }

        [Fact]
        public void CanGetMovesForPositionWithEnpassantBlack()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_00_01_00_00_00,
                BlackPawns = 0x00_00_00_00_02_00_00_00
            };

            var position = new Position(board, Side.Black, Square.a3, CastlingRights.None, 0);

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            moves.Should().HaveCount(2);
            moves.Where(m => m.Flags == MoveFlags.EnPassantCapture).Should().HaveCount(1);
        }

        [Fact]
        public void CanGetMovesForPositionWith2EnpassantWhite()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_14_00_00_00_00,
                BlackPawns = 0x00_00_00_08_00_00_00_00
            };

            var position = new Position(board, Side.White, Square.d6, CastlingRights.None, 0);

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            moves.Should().HaveCount(4);
            moves.Where(m => m.Flags == MoveFlags.EnPassantCapture).Should().HaveCount(2);
        }

        [Fact]
        public void GeneratesCorrectKnightMovesStartingPosition()
        {
            // Assemble
            var position = new Position(Board.StartingPosition, Side.White, null, CastlingRights.All, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddKnightMoves(position, moves);

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(Square.b1, Square.a3, MoveFlags.QuietMove),
                new Move(Square.b1, Square.c3, MoveFlags.QuietMove),
                new Move(Square.g1, Square.f3, MoveFlags.QuietMove),
                new Move(Square.g1, Square.h3, MoveFlags.QuietMove)
            };

            moves.Should().BeEquivalentTo(expectedMoves);
        }

        [Fact]
        public void GeneratesCorrectKnightMovesComplexPosition()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_00_00_00_ff_00,
                WhiteKnights = 0x00_00_00_02_00_10_00_00,
                BlackPawns = 0x00_17_a8_40_00_00_00_00,
                BlackBishops = 0x00_00_01_20_00_00_00_00
            };
            var position = new Position(board, Side.White, null, CastlingRights.None, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddKnightMoves(position, moves);

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(Square.e3, Square.d1, MoveFlags.QuietMove),
                new Move(Square.e3, Square.f1, MoveFlags.QuietMove),
                new Move(Square.e3, Square.c4, MoveFlags.QuietMove),
                new Move(Square.e3, Square.d5, MoveFlags.QuietMove),
                new Move(Square.e3, Square.g4, MoveFlags.QuietMove),
                new Move(Square.e3, Square.f5, MoveFlags.Capture),

                new Move(Square.b5, Square.a3, MoveFlags.QuietMove),
                new Move(Square.b5, Square.c3, MoveFlags.QuietMove),
                new Move(Square.b5, Square.d4, MoveFlags.QuietMove),
                new Move(Square.b5, Square.d6, MoveFlags.Capture),
                new Move(Square.b5, Square.a7, MoveFlags.Capture),
                new Move(Square.b5, Square.c7, MoveFlags.Capture),
            };

            moves.Should().BeEquivalentTo(expectedMoves);
        }

        [Fact]
        public void GeneratesCorrectBishopMovesWhite()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_00_00_29_d6_00,
                WhiteBishops = 0x00_00_00_00_02_40_00_00,
                BlackPawns = 0x00_d5_22_10_00_00_00_00,
                BlackBishops = 0x00_00_00_00_09_00_00_00
            };

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddBishopMoves(position, moves);

            // Assert
            moves.Should().HaveCount(12);
            moves.Where(m => m.From == Square.b4).Should().HaveCount(7);
            moves.Where(m => m.From == Square.g3).Should().HaveCount(5);
            moves.Where(m => m.IsCapture).Should().HaveCount(2);
        }

        [Fact]
        public void GeneratesCorrectBishopMovesBlack()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_00_00_29_d6_00,
                WhiteBishops = 0x00_00_00_00_02_40_00_00,
                BlackPawns = 0x00_d5_22_10_00_00_00_00,
                BlackBishops = 0x00_00_00_00_09_00_00_00
            };

            var position = new Position(board, Side.Black, null, CastlingRights.None, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddBishopMoves(position, moves);

            // Assert
            moves.Should().HaveCount(12);
            moves.Where(m => m.From == Square.a4).Should().HaveCount(6);
            moves.Where(m => m.From == Square.d4).Should().HaveCount(6);
            moves.Where(m => m.IsCapture).Should().HaveCount(2);
        }

        [Fact]
        public void GeneratesCorrectRookMovesWhite()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_00_80_01_76_00,
                WhiteRooks = 0x00_00_00_00_00_80_00_08,
                WhiteKnights = 0x00_00_20_02_00_00_00_62,
                BlackPawns = 0x00_28_13_84_00_00_00_00,
                BlackRooks = 0x00_10_00_00_00_02_00_00
            };

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddRookMoves(position, moves);

            // Assert
            moves.Should().HaveCount(16);
            moves.Where(m => m.From == Square.d1).Should().HaveCount(8);
            moves.Where(m => m.From == Square.h3).Should().HaveCount(8);
            moves.Where(m => m.IsCapture).Should().HaveCount(2);
        }

        [Fact]
        public void GeneratesCorrectRookMovesBlack()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_00_80_01_76_00,
                WhiteRooks = 0x00_00_00_00_00_80_00_08,
                WhiteKnights = 0x00_00_20_02_00_00_00_62,
                BlackPawns = 0x00_28_13_84_00_00_00_00,
                BlackRooks = 0x00_10_00_00_00_02_00_00
            };

            var position = new Position(board, Side.Black, null, CastlingRights.None, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddRookMoves(position, moves);

            // Assert
            moves.Should().HaveCount(11);
            moves.Where(m => m.From == Square.b3).Should().HaveCount(10);
            moves.Where(m => m.From == Square.e7).Should().HaveCount(1);
            moves.Where(m => m.IsCapture).Should().HaveCount(4);
        }


        [Fact]
        public void GeneratesCorrectQueenMovesWhite()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_08_00_10_c1_00,
                WhiteRooks = 0x00_00_00_00_00_20_00_08,
                WhiteKnights = 0x00_00_00_00_80_00_00_00,
                WhiteQueens = 0x00_00_10_00_00_00_00_00,
                WhiteKing = 0x00_00_00_00_00_00_00_40,
                BlackPawns = 0x00_c2_28_10_00_04_00_00,
                BlackBishops = 0x00_00_00_00_02_00_00_00,
                BlackRooks = 0x80_00_00_00_00_00_00_00,
                BlackQueens = 0x00_00_00_00_02_00_00_00,
                BlackKing = 0x00_00_80_00_00_00_00_00
            };

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddQueenMoves(position, moves);

            // Assert
            moves.Should().HaveCount(12);
            moves.Where(m => m.From == Square.e6).Should().HaveCount(12);
            moves.Where(m => m.IsCapture).Should().HaveCount(3);
        }

        [Fact]
        public void GeneratesCorrectQueenMovesBlack()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_08_00_10_c1_00,
                WhiteRooks = 0x00_00_00_00_00_20_00_08,
                WhiteKnights = 0x00_00_00_00_80_00_00_00,
                WhiteQueens = 0x00_00_10_00_00_00_00_00,
                WhiteKing = 0x00_00_00_00_00_00_00_40,
                BlackPawns = 0x00_c2_28_10_00_04_00_00,
                BlackBishops = 0x00_00_00_00_02_00_00_00,
                BlackRooks = 0x80_00_00_00_00_00_00_00,
                BlackQueens = 0x00_00_00_00_02_00_00_00,
                BlackKing = 0x00_00_80_00_00_00_00_00
            };

            var position = new Position(board, Side.Black, null, CastlingRights.None, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddQueenMoves(position, moves);

            // Assert
            moves.Should().HaveCount(15);
            moves.Where(m => m.From == Square.b4).Should().HaveCount(15);
            moves.Where(m => m.IsCapture).Should().HaveCount(1);
        }

        [Fact]
        public void GeneratesCorrectKingMovesWhite()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_00_00_00_00_00_c0_20,
                WhiteKing = 0x00_00_00_00_00_00_00_40,
                BlackKnights = 0x00_10_00_00_00_00_00_00,
                BlackKing = 0x10_00_00_00_00_00_00_00
            };

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddKingMoves(position, moves);

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(Square.g1, Square.h1, MoveFlags.QuietMove),
                new Move(Square.g1, Square.f2, MoveFlags.QuietMove)
            };

            moves.Should().BeEquivalentTo(expectedMoves);
        }

        [Fact]
        public void GeneratesCorrectKingMovesBlack()
        {
            // Assemble
            var board = new Board
            {
                WhitePawns = 0x00_08_00_00_00_00_c0_20,
                WhiteKing = 0x00_00_00_00_00_00_00_40,
                BlackKnights = 0x00_10_00_00_00_00_00_00,
                BlackKing = 0x10_00_00_00_00_00_00_00
            };

            var position = new Position(board, Side.Black, null, CastlingRights.None, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddKingMoves(position, moves);

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(Square.e8, Square.d7, MoveFlags.Capture),
                new Move(Square.e8, Square.f7, MoveFlags.QuietMove),
                new Move(Square.e8, Square.d8, MoveFlags.QuietMove),
                new Move(Square.e8, Square.f8, MoveFlags.QuietMove)
            };

            moves.Should().BeEquivalentTo(expectedMoves);
        }

        [Fact]
        public void GeneratesNoCastlingMovesWhenNotAvailable()
        {
            // Assemble
            var board = new Board
            {
                WhiteKing = 0x00_00_00_00_00_00_00_10,
                WhiteRooks = 0x00_00_00_00_00_00_00_81
            };

            var position = new Position(board, Side.White, null, CastlingRights.None, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddCastlingMoves(position, moves);

            // Assert
            moves.Should().BeEmpty();
        }

        [Fact]
        public void GeneratesCastlingMovesWhenAvailable()
        {
            // Assemble
            var board = new Board
            {
                WhiteKing = 0x00_00_00_00_00_00_00_10,
                WhiteRooks = 0x00_00_00_00_00_00_00_81
            };

            var position = new Position(board, Side.White, null, CastlingRights.WhiteBoth, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddCastlingMoves(position, moves);

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(Square.e1, Square.g1, MoveFlags.KingCastle),
                new Move(Square.e1, Square.c1, MoveFlags.QueenCastle),
            };

            moves.Should().BeEquivalentTo(expectedMoves);
        }

        [Fact]
        public void NoCastlingMovesWhenBlocked()
        {
            // Assemble
            var board = new Board
            {
                WhiteKing = 0x00_00_00_00_00_00_00_10,
                WhiteRooks = 0x00_00_00_00_00_00_00_80,
                WhiteKnights = 0x00_00_00_00_00_00_00_40
            };

            var position = new Position(board, Side.White, null, CastlingRights.WhiteKingSide, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddCastlingMoves(position, moves);

            // Assert
            moves.Should().BeEmpty();
        }

        [Fact]
        public void NoCastlingWhenInCheck()
        {
            // Assemble
            var board = new Board
            {
                WhiteKing = 0x00_00_00_00_00_00_00_10,
                WhiteRooks = 0x00_00_00_00_00_00_00_80,
                BlackBishops = 0x00_00_00_00_02_00_00_00
            };

            var position = new Position(board, Side.White, null, CastlingRights.WhiteKingSide, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddCastlingMoves(position, moves);

            // Assert
            moves.Should().BeEmpty();
        }

        [Fact]
        public void NoCastlingWhenMovingThroughCheck()
        {
            // Assemble
            var board = new Board
            {
                WhiteKing = 0x00_00_00_00_00_00_00_10,
                WhiteRooks = 0x00_00_00_00_00_00_00_80,
                BlackRooks = 0x20_00_00_00_00_00_00_00
            };

            var position = new Position(board, Side.White, null, CastlingRights.WhiteKingSide, 0);
            var moves = new List<Move>();

            // Act
            MoveGenerator.AddCastlingMoves(position, moves);

            // Assert
            moves.Should().BeEmpty();
        }

        [Fact]
        public void GeneratesCorrectMovesForStartingPosition()
        {
            // Assemble
            var position = new Position(Board.StartingPosition, Side.White, null, CastlingRights.All, 0);

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(Square.a2, Square.a3, MoveFlags.QuietMove),
                new Move(Square.b2, Square.b3, MoveFlags.QuietMove),
                new Move(Square.c2, Square.c3, MoveFlags.QuietMove),
                new Move(Square.d2, Square.d3, MoveFlags.QuietMove),
                new Move(Square.e2, Square.e3, MoveFlags.QuietMove),
                new Move(Square.f2, Square.f3, MoveFlags.QuietMove),
                new Move(Square.g2, Square.g3, MoveFlags.QuietMove),
                new Move(Square.h2, Square.h3, MoveFlags.QuietMove),

                new Move(Square.a2, Square.a4, MoveFlags.DoublePawnPush),
                new Move(Square.b2, Square.b4, MoveFlags.DoublePawnPush),
                new Move(Square.c2, Square.c4, MoveFlags.DoublePawnPush),
                new Move(Square.d2, Square.d4, MoveFlags.DoublePawnPush),
                new Move(Square.e2, Square.e4, MoveFlags.DoublePawnPush),
                new Move(Square.f2, Square.f4, MoveFlags.DoublePawnPush),
                new Move(Square.g2, Square.g4, MoveFlags.DoublePawnPush),
                new Move(Square.h2, Square.h4, MoveFlags.DoublePawnPush),

                new Move(Square.b1, Square.a3, MoveFlags.QuietMove),
                new Move(Square.b1, Square.c3, MoveFlags.QuietMove),
                new Move(Square.g1, Square.f3, MoveFlags.QuietMove),
                new Move(Square.g1, Square.h3, MoveFlags.QuietMove),
            };

            moves.Should().BeEquivalentTo(expectedMoves);
        }

        [Fact]
        public void GeneratesCorrectNumberOfMovesForKiwiPetePosition()
        {
            // Assemble
            var position = Position.FromFENString("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq -");

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            moves.Should().HaveCount(48);
            moves.Where(m => m.IsCapture).Should().HaveCount(8);
        }

        [Fact]
        public void GeneratesCorrectNumberOfMovesForPositionWithPossibleIllegalMoves()
        {
            // Assemble
            var position = Position.FromFENString("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - -");

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            moves.Should().HaveCount(14);
            moves.Where(m => m.IsCapture).Should().HaveCount(1);
        }

        [Fact]
        public void GeneratesCorrectNumberOfMovesForPositionWithPossiblePromotions()
        {
            // Assemble
            var position = Position.FromFENString("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8");

            // Act
            var moves = MoveGenerator.GetAllMoves(position);

            // Assert
            moves.Should().HaveCount(44);
        }
    }
}
