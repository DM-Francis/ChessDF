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
    public class MoverTests
    {
        [Fact]
        public void CanMakeMoveOnStartingPosition()
        {
            // Assemble
            var startingPosition = new Position(Board.StartingPosition, Side.White, null, CastlingRights.All, 0);
            var move = new Move(Square.e2, Square.e4, MoveFlags.DoublePawnPush);

            // Act
            var newPosition = startingPosition.MakeMove(move);

            // Assert
            var expectedBoard = new Board
            {
                WhitePawns = new Bitboard(0x00_00_00_00_10_00_ef_00),
                WhiteKnights = new Bitboard(0x00_00_00_00_00_00_00_42),
                WhiteBishops = new Bitboard(0x00_00_00_00_00_00_00_24),
                WhiteRooks = new Bitboard(0x00_00_00_00_00_00_00_81),
                WhiteQueens = new Bitboard(0x00_00_00_00_00_00_00_08),
                WhiteKing = new Bitboard(0x00_00_00_00_00_00_00_10),

                BlackPawns = new Bitboard(0x00_FF_00_00_00_00_00_00),
                BlackKnights = new Bitboard(0x42_00_00_00_00_00_00_00),
                BlackBishops = new Bitboard(0x24_00_00_00_00_00_00_00),
                BlackRooks = new Bitboard(0x81_00_00_00_00_00_00_00),
                BlackQueens = new Bitboard(0x08_00_00_00_00_00_00_00),
                BlackKing = new Bitboard(0x10_00_00_00_00_00_00_00)
            };

            newPosition.SideToMove.Should().Be(Side.Black);
            newPosition.EnPassantSquare.Should().Be(Square.e3);
            newPosition.HalfmoveClock.Should().Be(0);
            newPosition.CastlingRights.Should().Be(CastlingRights.All);
            newPosition.Board.Should().Be(expectedBoard);
        }

        [Fact]
        public void CreatesCorrectPositionAfterCastling()
        {
            // Assemble
            var position = Position.FromFENString("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8");
            var move = new Move(Square.e1, Square.g1, MoveFlags.KingCastle);

            // Act
            var newPosition = Mover.MakeMove(position, move);

            // Assert
            var expectedBoard = Board.FromPiecePlacementString("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQ1RK1");

            newPosition.SideToMove.Should().Be(Side.Black);
            newPosition.EnPassantSquare.Should().BeNull();
            newPosition.HalfmoveClock.Should().Be(2);
            newPosition.CastlingRights.Should().Be(CastlingRights.None);
            newPosition.Board.Should().Be(expectedBoard);
        }
    }
}
