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

        public ReadOnlyCollection<SearchResult> Search(Position position, int depth, IEnumerable<Move>? orderedMoves = null, CancellationToken cancelToken = default)
        {
            if (depth <= 0)
                throw new ArgumentOutOfRangeException(nameof(depth), $"{depth} must be one or more.");

            double alpha = -6000;
            double beta = 6000;
            _moveScores.Clear();

            List<Move> allMoves;
            if (orderedMoves is null)
                allMoves = MoveGenerator.GetAllMoves(position);
            else
                allMoves = orderedMoves.ToList();

            for (int i = 0; i < allMoves.Count; i++)
            {
                Move move = allMoves[i];
                Position newPosition = position.MakeMove(move);
                double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1, cancelToken);
                Mover.UndoMoveOnBoard(newPosition.Board, move);
                
                _output?.WriteDebug($"Evaluated move {move}. Score = {score} Depth = {depth}");
                _moveScores.Add(new SearchResult(move, score, i));
            }

            return MoveScores;
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
                _nodeCache[hash] = new Node(depth, score, NodeType.Exact);
                return score;
            }

            var allMoves = MoveGenerator.GetAllMoves(position, onlyLegal: false);
            NodeType nodeType = NodeType.UpperBound;
            foreach (Move move in allMoves)
            {
                Position newPosition = position.MakeMoveNoLegalCheck(move);
                double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1, cancelToken);
                Mover.UndoMoveOnBoard(newPosition.Board, move);

                if (score >= beta)
                {
                    _nodeCache[hash] = new Node(depth, beta, NodeType.LowerBound);
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

            _nodeCache[hash] = new Node(depth, alpha, nodeType);

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
                    _nodeCache[hash] = new Node(depth, beta, NodeType.LowerBound);
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
            _nodeCache[hash] = new Node(depth, alpha, nodeType);

            return alpha;
        }
    }
}
