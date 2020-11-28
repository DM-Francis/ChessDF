using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Uci;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ReadOnlyCollection<(Move move, double score)> BestMoves { get; }

        public Search(IEvaluator evaluator, IOutput? output = null)
        {
            _evaluator = evaluator;
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

            double max = -5000;

            var allMoves = MoveGenerator.GetAllMoves(position);
            allMoves.Reverse();
            foreach (Move move in allMoves)
            {
                Position newPosition = position.MakeMove(move);
                double score = -AlphaBeta(newPosition, double.MinValue, -max + 0.5, depth - 1);
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

        internal double AlphaBeta(Position position, double alpha, double beta, int depth)
        {
            if (depth == 0)
            {
                return _evaluator.Evaluate(position);
            }

            var allMoves = MoveGenerator.GetAllMoves(position, onlyLegal: false);

            foreach (Move move in allMoves)
            {
                Position newPosition = position.MakeMoveNoLegalCheck(move);
                double score = -AlphaBeta(newPosition, -beta, -alpha, depth - 1);
                Mover.UndoMoveOnBoard(newPosition.Board, move);

                if (score >= beta)
                    return beta;
                if (score > alpha)
                {
                    alpha = score;                                        
                }
            }

            return alpha;
        }
    }
}
