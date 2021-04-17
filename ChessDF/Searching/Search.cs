using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Uci;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChessDF.Searching
{
    internal class Line
    {
        public int MoveCount { get; set; }
        public List<Move> Moves { get; } = new List<Move>();
    }

    internal class Search
    {
        private readonly IEvaluator _evaluator;
        private readonly IOutput? _output;
        private readonly List<(Move move, double score)> _bestMoves = new List<(Move move, double score)>();
        private readonly ZobristGenerator _hashGenerator = new();
        private readonly Dictionary<ulong, Node> _nodeCache;

        public ReadOnlyCollection<(Move move, double score)> BestMoves { get; }

        public Search(IEvaluator evaluator, IOutput? output = null)
        {
            _evaluator = evaluator;
            _nodeCache = new();
            _hashGenerator = new();
            _output = output;
            BestMoves = _bestMoves.AsReadOnly();
        }

        public Search(IEvaluator evaluator, Dictionary<ulong, Node> nodeCache, ZobristGenerator generator, IOutput? output = null)
        {
            _evaluator = evaluator;
            _nodeCache = nodeCache;
            _hashGenerator = generator;
            _output = output;
            BestMoves = _bestMoves.AsReadOnly();
        }

        public ReadOnlyCollection<(Move move, double score)> SearchNegaMax(Position position, int depth)
        {
            if (depth <= 0)
                throw new ArgumentOutOfRangeException(nameof(depth), $"{depth} must be one or more.");

            var allMoves = MoveGenerator.GetAllMoves(position);

            double max = double.MinValue;

            foreach (Move move in allMoves)
            {
                Position newPosition = position.MakeMove(move);
                double score = -NegaMax(newPosition, depth - 1);
                Mover.UndoMoveOnBoard(newPosition.Board, move);

                _output?.WriteDebug($"Evaluated move {move}. Score = {score}");

                if (score > max)
                {
                    max = score;
                    _bestMoves.Clear();
                    _bestMoves.Add((move, score));
                }
                else if (score == max)
                {
                    _bestMoves.Add((move, score));
                }
            }

            return BestMoves;
        }

        internal double NegaMax(Position position, int depth)
        {
            if (depth == 0)
                return _evaluator.Evaluate(position);

            double max = double.MinValue;
            var allMoves = MoveGenerator.GetAllMoves(position);

            foreach (Move move in allMoves)
            {
                Position newPosition = position.MakeMove(move);
                double score = -NegaMax(newPosition, depth - 1);
                Mover.UndoMoveOnBoard(newPosition.Board, move);

                if (score > max)
                    max = score;
            }

            return max;
        }

        public ReadOnlyCollection<(Move move, double score)> SearchAlphaBeta(Position position, int depth)
        {
            if (depth <= 0)
                throw new ArgumentOutOfRangeException(nameof(depth), $"{depth} must be one or more.");

            double alpha = -6000;
            double beta = 6000;

            var allMoves = MoveGenerator.GetAllMoves(position);
            foreach (Move move in allMoves)
            {
                Position newPosition = position.MakeMove(move);
                double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1);
                Mover.UndoMoveOnBoard(newPosition.Board, move);

                _output?.WriteDebug($"Evaluated move {move}. Score = {score}");

                if (score > alpha)
                {
                    alpha = score;
                    _bestMoves.Clear();
                    _bestMoves.Add((move, score));
                }
                //else if (score == alpha)
                //{
                //    _bestMoves.Add((move, score));
                //}
            }

            return BestMoves;
        }

        internal double AlphaBeta(Position position, double alpha, double beta, int depth)
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
                double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1);
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
