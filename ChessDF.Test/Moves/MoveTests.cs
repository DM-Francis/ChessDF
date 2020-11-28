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
    public class MoveTests
    {
        [Fact]
        public void PropertiesAreConsistentWithConstructor()
        {
            // Assemble
            Square from = Square.a6;
            Square to = Square.a5;
            MoveFlags flags = MoveFlags.QuietMove;

            // Act
            var move = new Move(from, to, flags);

            // Assert
            Assert.Equal(from, move.From);
            Assert.Equal(to, move.To);
            Assert.Equal(flags, move.Flags);
        }

        [Fact]
        public void CapturedPieceIsNullForNonCaptures()
        {
            // Act & Assemble
            var move = new Move(Square.a2, Square.a4, MoveFlags.DoublePawnPush);

            // Assert
            Assert.Null(move.CapturedPiece);

        }

        [Fact]
        public void CapturePropertyWorksWithStandardCaptures()
        {
            // Assemble
            Square from = Square.c3;
            Square to = Square.e5;
            var flags = MoveFlags.Capture;

            // Act
            var move = new Move(from, to, flags, Piece.Pawn);

            // Assert
            Assert.True(move.IsCapture);
            Assert.Equal(Piece.Pawn, move.CapturedPiece);
        }

        [Fact]
        public void CapturePropertyWorksWithPromotionCaptures()
        {
            // Assemble
            Square from = Square.d7;
            Square to = Square.d8;
            var flags = MoveFlags.Capture | MoveFlags.QueenPromotion;

            // Act
            var move = new Move(from, to, flags, Piece.Rook);

            // Assert
            Assert.True(move.IsCapture);
            Assert.Equal(Piece.Rook, move.CapturedPiece);
        }

        public static IEnumerable<object[]> TestMoves()
        {
            string position1 = "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1";
            string position2 = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - ";
            string position3 = "8/4p1k1/p2p1ppp/1p6/1Pp5/4P3/P1PP1PPP/3K4 b - b3 0 1 ";
            string position4 = "r2qkbnr/pppbpp1p/2np2p1/8/4P3/1P1P4/P1P1QPPP/RNB1KBNR w KQkq - 3 5 ";

            yield return new object[] { "b4a5", position1, new Move(Square.b4, Square.a5, MoveFlags.Capture, Piece.Knight) };
            yield return new object[] { "h2h4", position1, new Move(Square.h2, Square.h4, MoveFlags.DoublePawnPush) };
            yield return new object[] { "e8c8", position1, new Move(Square.e8, Square.c8, MoveFlags.QueenCastle) };
            yield return new object[] { "f6e4", position1, new Move(Square.f6, Square.e4, MoveFlags.Capture, Piece.Pawn) };
            yield return new object[] { "b2b1r", position1, new Move(Square.b2, Square.b1, MoveFlags.RookPromotion) };
            yield return new object[] { "b2a1q", position1, new Move(Square.b2, Square.a1, MoveFlags.Capture | MoveFlags.QueenPromotion, Piece.Rook) };
            yield return new object[] { "e1g1", position2, new Move(Square.e1, Square.g1, MoveFlags.KingCastle) };
            yield return new object[] { "e1c1", position2, new Move(Square.e1, Square.c1, MoveFlags.QueenCastle) };
            yield return new object[] { "e8g8", position2, new Move(Square.e8, Square.g8, MoveFlags.KingCastle) };
            yield return new object[] { "e8c8", position2, new Move(Square.e8, Square.c8, MoveFlags.QueenCastle) };
            yield return new object[] { "b4c3", position2, new Move(Square.b4, Square.c3, MoveFlags.Capture, Piece.Knight) };
            yield return new object[] { "c4b3", position3, new Move(Square.c4, Square.b3, MoveFlags.EnPassantCapture, Piece.Pawn) };
            yield return new object[] { "e4e5", position4, new Move(Square.e4, Square.e5, MoveFlags.QuietMove) };
        }

        [Theory]
        [MemberData(nameof(TestMoves))]
        public void CanCreateMovesFromStringAndPosition(string moveString, string positionString, Move expectedMove)
        {
            // Assemble
            var position = Position.FromFENString(positionString);

            // Act
            var move = Move.FromStringAndPosition(moveString, position);

            // Assert
            Assert.Equal(expectedMove, move);
        }

        [Theory]
        [InlineData(MoveFlags.KnightPromotion, Piece.Knight)]
        [InlineData(MoveFlags.BishopPromotion, Piece.Bishop)]
        [InlineData(MoveFlags.RookPromotion, Piece.Rook)]
        [InlineData(MoveFlags.QueenPromotion, Piece.Queen)]        
        [InlineData(MoveFlags.QuietMove, null)]
        public void HasCorrectPromotionPieceNonCaptures(MoveFlags flags, Piece? expectedPromotionPiece)
        {
            // Assemble
            var move = new Move(Square.e7, Square.e8, flags);

            // Act & Assert
            Assert.Equal(expectedPromotionPiece, move.PromotionPiece);
        }

        [Theory]
        [InlineData(MoveFlags.Capture | MoveFlags.KnightPromotion, Piece.Knight)]
        [InlineData(MoveFlags.Capture | MoveFlags.BishopPromotion, Piece.Bishop)]
        [InlineData(MoveFlags.Capture | MoveFlags.RookPromotion, Piece.Rook)]
        [InlineData(MoveFlags.Capture | MoveFlags.QueenPromotion, Piece.Queen)]
        public void HasCorrectPromotionPieceCaptures(MoveFlags flags, Piece? expectedPromotionPiece)
        {
            // Assemble
            var move = new Move(Square.e7, Square.e8, flags, Piece.Bishop);

            // Act & Assert
            Assert.Equal(expectedPromotionPiece, move.PromotionPiece);
        }

        [Fact]
        public void ThrowsWhenCaptureFlagSetUsingWrongConstructor()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Move(Square.a1, Square.d4, MoveFlags.Capture));
        }

        [Fact]
        public void ThrowsWhenCaptureFlagNotSetUsingWrongConstructor()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Move(Square.a1, Square.d4, MoveFlags.QuietMove, Piece.Bishop));
        }
    }
}
