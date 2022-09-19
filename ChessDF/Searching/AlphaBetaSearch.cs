using System;
using System.Linq;
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
    private readonly Random _rng = new();

    internal AlphaBetaSearch(IEvaluator evaluator, IOutput? output = null)
    {
        _evaluator = evaluator;
        _output = output;
    }

    public Move BestMove { get; private set; }
    public int NodesSearched { get; private set; }

    public void Search(Position position, int depth, CancellationToken cancellationToken = default)
    {
        if (depth <= 0)
            throw new ArgumentOutOfRangeException(nameof(depth), $"{depth} must be one or more.");

        double alpha = double.NegativeInfinity;
        double beta = double.PositiveInfinity;

        NodesSearched = 1;
        int moveNum = 0;
        foreach (var move in position.GetAllLegalMoves().OrderBy(x => _rng.Next()))
        {
            moveNum++;
            _output?.WriteCurrentMoveInfo(move, moveNum);
            Position newPosition = position.MakeMoveNoLegalCheck(move);
            double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1);
            Mover.UndoMoveOnBoard(newPosition.Board, move);
            
            if (score > alpha)
            {
                alpha = score;
                BestMove = move;
            }

            _output?.WriteBestLineInfo(depth, alpha, NodesSearched, new []{BestMove});
        }

        _output?.WriteDebug($"Best move: {BestMove}. Nodes searched: {NodesSearched}");
    }

    internal double AlphaBeta(Position position, double alpha, double beta, int depth)
    {
        NodesSearched++;
        if (position.IsInCheckmate())
            return double.NegativeInfinity;

        if (position.IsInStalemate())
            return 0;

        if (depth == 0)
        {
            return Quiese(position, alpha, beta, 0, -3);
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

    internal double Quiese(Position position, double alpha, double beta, int depth, int maxDepth)
    {
        NodesSearched++;
        double standPat = _evaluator.Evaluate(position);

        if (depth == maxDepth)
            return standPat;

        if (standPat >= beta)
            return beta;
        if (alpha < standPat)
            alpha = standPat;

        var allMoves = MoveGenerator.GetAllMoves(position, onlyLegal: false);
        foreach (Move move in allMoves)
        {
            if (!move.IsCapture)
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