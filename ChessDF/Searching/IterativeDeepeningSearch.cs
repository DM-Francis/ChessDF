using System;
using System.Threading;
using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Uci;

namespace ChessDF.Searching;

public class IterativeDeepeningSearch : ISearch
{
    private readonly IEvaluator _evaluator;
    private readonly IOutput? _output;
    private readonly AlphaBetaSearch _alphaBeta;

    internal IterativeDeepeningSearch(IEvaluator evaluator, IOutput? output = null)
    {
        _evaluator = evaluator;
        _output = output;
        _alphaBeta = new AlphaBetaSearch(evaluator, output);
    }
    
    public Move BestMove { get; private set; }
    public int NodesSearched => _alphaBeta.NodesSearched;
    
    public void Search(Position position, int maxDepth, CancellationToken cancellationToken = default)
    {
        if (maxDepth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxDepth), $"{maxDepth} must be one or more.");

        for (int d = 1; d <= maxDepth; d++)
        {
            _alphaBeta.SearchFirst = BestMove;
            _alphaBeta.Search(position, d, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return;
            
            BestMove = _alphaBeta.BestMove;
        }
    }
}