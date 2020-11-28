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
        public void CreatesCorrectPositionAfterCastlingKingSide()
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

        [Fact]
        public void CreatesCorrectPositionAfterCastlingQueenSide()
        {
            // Assemble
            var position = Position.FromFENString("rnbq1k1r/pp1Pbppp/2p5/8/1QB2B2/2N5/PP2N1PP/R3K2R w KQ - 2 8 ");
            var move = new Move(Square.e1, Square.c1, MoveFlags.QueenCastle);

            // Act
            var newPosition = Mover.MakeMove(position, move);

            // Assert
            var expectedPosition = Position.FromFENString("rnbq1k1r/pp1Pbppp/2p5/8/1QB2B2/2N5/PP2N1PP/2KR3R b - - 3 8 ");

            Assert.Equal(expectedPosition, newPosition);
        }

        [Fact]
        public void CorrectlyUpdatesBoardWithPromotion()
        {
            // Assemble
            var position = Position.FromFENString("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8");
            var move = new Move(Square.d7, Square.c8, MoveFlags.Capture | MoveFlags.KnightPromotion, Piece.Bishop);

            // Act
            var newPosition = Mover.MakeMove(position, move);

            // Assert
            var expectedPosition = Position.FromFENString("rnNq1k1r/pp2bppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R b KQ - 0 8");

            Assert.Equal(expectedPosition, newPosition);
        }


        [Fact]
        public void CastlingRightsAreCorrectAfterMovingKingRook()
        {
            // Assemble
            var position = Position.FromFENString("rnbq1k1r/pp1Pbppp/2p5/8/1QB2B2/2N5/PP2N1PP/R3K2R w KQ - 2 8 ");
            var move = new Move(Square.h1, Square.f1, MoveFlags.QuietMove);

            // Act
            var newPosition = Mover.MakeMove(position, move);

            // Assert
            var expectedPosition = Position.FromFENString("rnbq1k1r/pp1Pbppp/2p5/8/1QB2B2/2N5/PP2N1PP/R3KR2 b Q - 3 8 ");

            Assert.Equal(expectedPosition, newPosition);
        }

        [Fact]
        public void CastlingRightsAreCorrectAfterMovingQueenRook()
        {
            // Assemble
            var position = Position.FromFENString("r3kbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1 ");
            var move = new Move(Square.a8, Square.c8, MoveFlags.QuietMove);

            // Act
            var newPosition = Mover.MakeMove(position, move);

            // Assert
            var expectedPosition = Position.FromFENString("2r1kbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQk - 1 2 ");

            Assert.Equal(expectedPosition, newPosition);
        }

        [Fact]
        public void CastlingRightsAreCorrectAfterRookIsCaptured()
        {
            // Assemble
            var position = Position.FromFENString("rnbqkbnr/ppppppPp/8/8/8/8/PPPPPP1P/RNBQKBNR w KQkq - 0 1 ");
            var move = new Move(Square.g7, Square.h8, MoveFlags.Capture | MoveFlags.BishopPromotion, Piece.Rook);

            // Act
            var newPosition = Mover.MakeMove(position, move);

            // Assert
            var expectedPosition = Position.FromFENString("rnbqkbnB/pppppp1p/8/8/8/8/PPPPPP1P/RNBQKBNR b KQq - 0 1 ");

            Assert.Equal(expectedPosition, newPosition);
        }

        [Fact]
        public void CanUnmakeCapturesOnBoard()
        {
            // Assemble
            var board = Board.FromPiecePlacementString("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R");
            var move = new Move(Square.g4, Square.f2, MoveFlags.Capture, Piece.Pawn);

            // Act
            Mover.UndoMoveOnBoard(board, move);

            // Assert
            string expectedBoard = "rnbq1k1r/pp1Pbppp/2p5/8/2B3n1/8/PPP1NPPP/RNBQK2R";
            Assert.Equal(expectedBoard, board.ToPiecePlacementString());
        }

        [Fact]
        public void CanUnmakeEnpassantOnBoard()
        {
            // Assemble
            var board = Board.FromPiecePlacementString("rnbq1k1r/pp1pb1pp/2p2P2/8/2B3n1/8/PPP1NPPP/RNBQK2R");
            var move = new Move(Square.e5, Square.f6, MoveFlags.EnPassantCapture, Piece.Pawn);

            // Act
            Mover.UndoMoveOnBoard(board, move);

            // Assert
            string expectedBoard = "rnbq1k1r/pp1pb1pp/2p5/4Pp2/2B3n1/8/PPP1NPPP/RNBQK2R";
            Assert.Equal(expectedBoard, board.ToPiecePlacementString());
        }

        [Fact]
        public void CanUnmakePromotionOnBoard()
        {
            // Assemble
            var board = Board.FromPiecePlacementString("rnbq1kNr/pp1pb2p/2p5/8/2B3n1/8/PPP1NPPP/RNBQK2R");
            var move = new Move(Square.g7, Square.g8, MoveFlags.KnightPromotion);

            // Act
            Mover.UndoMoveOnBoard(board, move);

            // Assert
            string expectedBoard = "rnbq1k1r/pp1pb1Pp/2p5/8/2B3n1/8/PPP1NPPP/RNBQK2R";
            Assert.Equal(expectedBoard, board.ToPiecePlacementString());
        }

        [Fact]
        public void CanUnmakePromotionCaptureOnBoard()
        {
            // Assemble
            var board = Board.FromPiecePlacementString("rnbq1k1Q/pp1pb2p/2p5/8/2B3n1/8/PPP1NPPP/RNBQK2R");
            var move = new Move(Square.g7, Square.h8, MoveFlags.QueenPromotion | MoveFlags.Capture, Piece.Rook);

            // Act
            Mover.UndoMoveOnBoard(board, move);

            // Assert
            string expectedBoard = "rnbq1k1r/pp1pb1Pp/2p5/8/2B3n1/8/PPP1NPPP/RNBQK2R";
            Assert.Equal(expectedBoard, board.ToPiecePlacementString());
        }

        public static IEnumerable<object[]> CastlingTestData()
        {
            string castlingBasePosition = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R";

            yield return new object[] { castlingBasePosition, "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R4RK1", new Move(Square.e1, Square.g1, MoveFlags.KingCastle) };
            yield return new object[] { castlingBasePosition, "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/2KR3R", new Move(Square.e1, Square.c1, MoveFlags.QueenCastle) };
            yield return new object[] { castlingBasePosition, "r4rk1/pppppppp/8/8/8/8/PPPPPPPP/R3K2R", new Move(Square.e8, Square.g8, MoveFlags.KingCastle) };
            yield return new object[] { castlingBasePosition, "2kr3r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R", new Move(Square.e8, Square.c8, MoveFlags.QueenCastle) };
        }


        [Theory]
        [MemberData(nameof(CastlingTestData))]
        public void CanUnmakeCastlingOnBoard(string beforeBoard, string afterBoard, Move move)
        {
            // Assemble
            Board board = Board.FromPiecePlacementString(afterBoard);

            // Act
            Mover.UndoMoveOnBoard(board, move);

            // Assert
            string expectedBoard = beforeBoard;
            Assert.Equal(expectedBoard, board.ToPiecePlacementString());
        }
    }
}
