using System;
using System.Threading;
using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Uci;

namespace ChessDF.Searching;

public class AlphaBetaSearch : ISearch
{
    private readonly IEvaluator _evaluator;
    private readonly IOutput? _output;

    internal AlphaBetaSearch(IEvaluator evaluator, IOutput? output = null)
    {
        _evaluator = evaluator;
        _output = output;
    }

    public Move BestMove { get; private set; }
    
    public void Search(Position position, int depth, CancellationToken cancellationToken = default)
    {
        if (depth <= 0)
            throw new ArgumentOutOfRangeException(nameof(depth), $"{depth} must be one or more.");

        double alpha = double.NegativeInfinity;
        double beta = double.PositiveInfinity;

        foreach (var move in position.GetAllLegalMoves())
        {
            Position newPosition = position.MakeMoveNoLegalCheck(move);
            double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1);
            Mover.UndoMoveOnBoard(newPosition.Board, move);
            
            _output?.WriteDebug($"Evaluated move {move}. Score = {score}");

            if (score > alpha)
            {
                alpha = score;
                BestMove = move;
            }
        }
    }

    internal double AlphaBeta(Position position, double alpha, double beta, int depth)
    {
        if (position.IsInCheckmate())
            return double.NegativeInfinity;

        if (position.IsInStalemate())
            return 0;
        
        if (depth == 0)
        {
            return _evaluator.Evaluate(position);
        }

        foreach (var move in position.GetAllLegalMoves())
        {
            Position newPosition = position.MakeMoveNoLegalCheck(move);
            double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1);
            Mover.UndoMoveOnBoard(newPosition.Board, move);

            if (score >= beta)
                return beta;
            if (score > alpha)
                alpha = score;
        }

        return alpha;
    }
}