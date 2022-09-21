using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Uci;

namespace ChessDF.Searching;

public class AlphaBetaSearch : ISearch
{
    private const double MinScore = -6000;
    private const double MaxScore = 6000;
    private const double MateValue = 4000;
    
    private readonly IEvaluator _evaluator;
    private readonly IOutput? _output;
    private readonly Random _rng = new();

    internal AlphaBetaSearch(IEvaluator evaluator, IOutput? output = null)
    {
        _evaluator = evaluator;
        _output = output;
    }

    public Move BestMove { get; private set; }
    public int NodesSearched { get; private set; }
    public Move? SearchFirst { get; set; }

    private List<Move> _principalVariation = new();
    public IReadOnlyCollection<Move> PrincialVariation => _principalVariation.AsReadOnly();

    public void Search(Position position, int maxDepth, CancellationToken cancellationToken = default)
    {
        if (maxDepth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxDepth), $"{maxDepth} must be one or more.");

        _principalVariation = new List<Move>();
        BestMove = default;
        double alpha = MinScore;
        double beta = MaxScore;

        NodesSearched = 1;
        int moveNum = 0;
        var allMovesOrdered = position
            .GetAllLegalMoves()
            .OrderByDescending(x => x == SearchFirst)
            .ThenByDescending(x => x.IsCapture)
            .ThenBy(x => _rng.Next());
        foreach (var move in allMovesOrdered)
        {
            moveNum++;
            _output?.WriteCurrentMoveInfo(move, moveNum);
            Position newPosition = position.MakeMoveNoLegalCheck(move);
            double score = -AlphaBeta(newPosition, -beta, -alpha, maxDepth - 1, out var line, cancellationToken);
            Mover.UndoMoveOnBoard(newPosition.Board, move);

            if (cancellationToken.IsCancellationRequested)
                return;
            
            if (score > alpha)
            {
                alpha = score;
                BestMove = move;
                _principalVariation.Clear();
                _principalVariation.Add(move);
                _principalVariation.AddRange(line);
            }

            _output?.WriteBestLineInfo(maxDepth, alpha, NodesSearched, _principalVariation);
        }

        _output?.WriteDebug($"Best move: {BestMove}. Nodes searched: {NodesSearched}");
    }

    internal double AlphaBeta(Position position, double alpha, double beta, int depth, out List<Move> principalLine, CancellationToken cancelToken = default)
    {
        principalLine = new List<Move>();

        NodesSearched++;
        if (position.IsInCheckmate())
            return -(MateValue + depth);  // Ensure quicker checkmates are better than later ones

        if (position.IsInStalemate())
            return 0;

        if (depth == 0)
        {
            return Quiese(position, alpha, beta, 0, -3);
        }

        var allMovesOrdered = position
            .GetAllLegalMoves()
            .OrderByDescending(x => x.IsCapture);

        foreach (var move in allMovesOrdered)
        {
            Position newPosition = position.MakeMoveNoLegalCheck(move);
            double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1, out var line, cancelToken);
            Mover.UndoMoveOnBoard(newPosition.Board, move);

            if (cancelToken.IsCancellationRequested)
                break;
            
            if (score >= beta)
                return beta;
            if (score > alpha)
            {
                alpha = score;
                principalLine.Clear();
                principalLine.Add(move);
                principalLine.AddRange(line);
            }
        }

        return alpha;
    }

    internal double Quiese(Position position, double alpha, double beta, int depth, int maxDepth)
    {
        NodesSearched++;
        double standPat = _evaluator.Evaluate(position);

        if (depth == maxDepth)
            return standPat;

        if (standPat >= beta)
            return beta;
        if (standPat > alpha)
            alpha = standPat;

        var allMoves = MoveGenerator.GetAllMoves(position, onlyLegal: false);
        foreach (Move move in allMoves)
        {
            if (!move.IsCapture)
                continue;

            double swingValue = move.CapturedPiece!.Value.ScoreValue();

            if (standPat + swingValue < alpha - 2) // Delta pruning.  If capturing can't get us close to alpha, then don't both analysing the move
                continue;

            Position newPosition = position.MakeMoveNoLegalCheck(move);
            double score = -Quiese(newPosition, -beta, -alpha, depth - 1, maxDepth);
            Mover.UndoMoveOnBoard(newPosition.Board, move);

            if (score >= beta)
                return beta;

            if (score > alpha) alpha = score;
        }

        return alpha;
    }
}