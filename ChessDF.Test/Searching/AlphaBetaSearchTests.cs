using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Searching;
using Xunit;

namespace ChessDF.Test.Searching;

public class AlphaBetaSearchTests
{
    [Fact]
    public void CanSearchWithAlphaBetaDepth5_BasicEval()
    {
        // Assemble
        var search = new AlphaBetaSearch(new BasicScoreEvaluation());
        var position = Position.FromFENString("2rr2k1/5Rp1/2b1p1Pp/1pq5/p4QN1/8/PPP5/1K3R2 b - - 0 1 ");

        // Act
        search.Search(position, 5);
        
        // Basic eval - searches 564426 nodes

        // Assert
        var expectedMove = new Move(Square.c5, Square.c2, MoveFlags.Capture, Piece.Pawn);
        Assert.Equal(expectedMove, search.BestMove);
        Assert.Equal(564426, search.NodesSearched);
    }
    
    [Fact]
    public void CanSearchWithAlphaBetaDepth3_BasicWithRandomnessEval()
    {
        // Assemble
        var search = new AlphaBetaSearch(new ScoreEvalWithRandomness());
        var position = Position.FromFENString("2rr2k1/5Rp1/2b1p1Pp/1pq5/p4QN1/8/PPP5/1K3R2 b - - 0 1 ");

        // Act
        search.Search(position, 3);
    }
}