using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Uci;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace ChessDF.Searching
{
    internal class Line
    {
        public int MoveCount { get; set; }
        public List<Move> Moves { get; } = new List<Move>();
    }

    internal class AlphaBetaSearch : ISearchStrategy
    {
        private readonly IEvaluator _evaluator;
        private readonly IOutput? _output;
        private readonly List<SearchResult> _moveScores = new();
        private readonly ZobristGenerator _hashGenerator = new();
        private readonly Dictionary<ulong, Node> _nodeCache;

        public ReadOnlyCollection<SearchResult> MoveScores => _moveScores.AsReadOnly();

        public AlphaBetaSearch(IEvaluator evaluator, IOutput? output = null)
        {
            _evaluator = evaluator;
            _nodeCache = new();
            _hashGenerator = new();
            _output = output;
        }

        public AlphaBetaSearch(IEvaluator evaluator, Dictionary<ulong, Node> nodeCache, ZobristGenerator generator, IOutput? output = null)
        {
            _evaluator = evaluator;
            _nodeCache = nodeCache;
            _hashGenerator = generator;
            _output = output;
        }

        public ReadOnlyCollection<SearchResult> Search(Position position, int depth, CancellationToken cancelToken = default)
        {
            if (depth <= 0)
                throw new ArgumentOutOfRangeException(nameof(depth), $"{depth} must be one or more.");

            double alpha = -6000;
            double beta = 6000;
            _moveScores.Clear();

            List<Move> allMoves = MoveGenerator.GetAllMoves(position);
            List<Move> orderedMoves = OrderMovesBasedOnPreviousSearches(position, allMoves);

            Move bestMove = orderedMoves[0];

            foreach (Move move in orderedMoves)
            {
                Position newPosition = position.MakeMove(move);
                double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1, cancelToken);
                Mover.UndoMoveOnBoard(newPosition.Board, move);
                
                _output?.WriteDebug($"Evaluated move {move}. Score = {score} Depth = {depth}");
                
                if (score > alpha)
                {
                    alpha = score;
                    bestMove = move;
                }
            }

            ulong hash = _hashGenerator.GetHash(position);
            _nodeCache[hash] = new Node(depth, alpha, NodeType.Exact, bestMove);

            var result = new SearchResult(bestMove, alpha, 1);

            return new List<SearchResult>() { result }.AsReadOnly();
        }

        internal double AlphaBeta(Position position, double alpha, double beta, int depth, CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();
            ulong hash = _hashGenerator.GetHash(position);
            if (_nodeCache.TryGetValue(hash, out Node? node) && node.Depth >= depth)
            {
                if (node.NodeType == NodeType.Exact)
                    return node.Score;
                else if (node.NodeType == NodeType.UpperBound)
                    beta = node.Score;
                else if (node.NodeType == NodeType.LowerBound)
                    alpha = node.Score;
            }

            if (depth == 0)
            {
                double score = Quiese(position, alpha, beta, 0, -3);
                _nodeCache[hash] = new Node(depth, score, NodeType.Exact, null);
                return score;
            }

            var allMoves = MoveGenerator.GetAllMoves(position, onlyLegal: false);
            Move? bestMove = null;

            NodeType nodeType = NodeType.UpperBound;
            foreach (Move move in allMoves)
            {
                Position newPosition = position.MakeMoveNoLegalCheck(move);
                double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1, cancelToken);
                Mover.UndoMoveOnBoard(newPosition.Board, move);

                if (score >= beta)
                {
                    _nodeCache[hash] = new Node(depth, beta, NodeType.LowerBound, move);
                    return beta;
                }

                if (score > alpha)
                {
                    nodeType = NodeType.Exact;
                    alpha = score;
                    bestMove = move;
                }
            }

            if (alpha <= -4000 && position.IsInStatemate())
                alpha = 0;

            _nodeCache[hash] = new Node(depth, alpha, nodeType, bestMove);

            return alpha;
        }


        internal double Quiese(Position position, double alpha, double beta, int depth, int maxDepth)
        {
            ulong hash = _hashGenerator.GetHash(position);
            if (_nodeCache.TryGetValue(hash, out Node? node) && node.Depth >= depth)
            {
                if (node.NodeType == NodeType.Exact)
                    return node.Score;
                else if (node.NodeType == NodeType.UpperBound)
                    beta = node.Score;
                else if (node.NodeType == NodeType.LowerBound)
                    alpha = node.Score;
            }

            double standPat = _evaluator.Evaluate(position);

            if (depth == maxDepth)
                return standPat;

            if (standPat >= beta)
                return beta;
            if (alpha < standPat)
                alpha = standPat;

            var allMoves = MoveGenerator.GetAllMoves(position, onlyLegal: false);
            Move? bestMove = null;

            NodeType nodeType = NodeType.UpperBound;
            foreach (Move move in allMoves)
            {
                if (!move.IsCapture)
                    continue;

                Position newPosition = position.MakeMoveNoLegalCheck(move);
                double score = -Quiese(newPosition, -beta, -alpha, depth - 1, maxDepth);
                Mover.UndoMoveOnBoard(newPosition.Board, move);

                if (score >= beta)
                {
                    _nodeCache[hash] = new Node(depth, beta, NodeType.LowerBound, move);
                    return beta;
                }

                if (score > alpha)
                {
                    nodeType = NodeType.Exact;
                    alpha = score;
                }
            }

            if (alpha <= -4000 && position.IsInStatemate())
                alpha = 0;
            _nodeCache[hash] = new Node(depth, alpha, nodeType, bestMove);

            return alpha;
        }

        private List<Move> OrderMovesBasedOnPreviousSearches(Position position, IEnumerable<Move> moves)
        {
            var moveScores = new List<(Move Move, Double Score)>();
            foreach (Move move in moves)
            {
                Position newPosition = position.MakeMoveNoLegalCheck(move);
                ulong hash = _hashGenerator.GetHash(newPosition);
                Mover.UndoMoveOnBoard(newPosition.Board, move);

                if (_nodeCache.TryGetValue(hash, out Node? node))
                {
                    if (node.NodeType == NodeType.Exact)
                        moveScores.Add((move, node.Score));
                    else if (node.NodeType == NodeType.UpperBound)
                        moveScores.Add((move, -6000));
                    else if (node.NodeType == NodeType.LowerBound)
                        moveScores.Add((move, node.Score));
                }
                else
                {
                    moveScores.Add((move, -6000));
                }
            }

            return moveScores.OrderByDescending(ms => ms.Score).Select(ms => ms.Move).ToList();
        }
    }
}
