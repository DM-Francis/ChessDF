using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Uci;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessDF.Searching
{
    internal class NegaMaxSearch : ISearchStrategy
    {
        private readonly IEvaluator _evaluator;
        private readonly IOutput? _output;
        private readonly List<SearchResult> _bestMoves = new();

        public ReadOnlyCollection<SearchResult> MoveScores => _bestMoves.AsReadOnly();


        public NegaMaxSearch(IEvaluator evaluator, IOutput? output = null)
        {
            _evaluator = evaluator;
            _output = output;
        }

        public ReadOnlyCollection<SearchResult> Search(Position position, int depth, IEnumerable<Move>? orderedMoves = null, CancellationToken cancelToken = default)
        {
            if (depth <= 0)
                throw new ArgumentOutOfRangeException(nameof(depth), $"{depth} must be one or more.");

            var allMoves = MoveGenerator.GetAllMoves(position);

            double max = double.MinValue;

            for (int i = 0; i < allMoves.Count; i++)
            {
                Move move = allMoves[i];
                Position newPosition = position.MakeMove(move);
                double score = -NegaMax(newPosition, depth - 1);
                Mover.UndoMoveOnBoard(newPosition.Board, move);

                _output?.WriteDebug($"Evaluated move {move}. Score = {score}");

                if (score > max)
                {
                    max = score;
                    _bestMoves.Clear();
                    _bestMoves.Add(new SearchResult(move, score, i));
                }
                else if (score == max)
                {
                    _bestMoves.Add(new SearchResult(move, score, i));
                }
            }

            return MoveScores;
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
    }
}
