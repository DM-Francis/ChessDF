using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Searching
{
    public class Search
    {
        private static readonly Random _rng = new();
        private readonly IEvaluator _evaluator;

        public Search(IEvaluator evaluator)
        {
            _evaluator = evaluator;
        }

        public Move RootNegaMax(Position position, int depth)
        {
            if (depth <= 0)
                throw new ArgumentOutOfRangeException(nameof(depth), $"{depth} must be one or more.");

            var allMoves = MoveGenerator.GetAllMoves(position);

            double max = double.MinValue;
            List<Move> bestMoves = new();

            foreach (Move move in allMoves)
            {
                Position newPosition = position.MakeMove(move);
                double score = -NegaMax(newPosition, depth - 1);
                if (score > max)
                {
                    max = score;
                    bestMoves.Clear();
                    bestMoves.Add(move);
                }
                else if (score == max)
                {
                    bestMoves.Add(move);
                }
            }

            return bestMoves[_rng.Next() % bestMoves.Count];
        }

        public double NegaMax(Position position, int depth)
        {
            if (depth == 0)
                return _evaluator.Evaluate(position);

            double max = double.MinValue;
            var allMoves = MoveGenerator.GetAllMoves(position);

            foreach (Move move in allMoves)
            {
                Position newPosition = position.MakeMove(move);
                double score = -NegaMax(newPosition, depth - 1);
                if (score > max)
                    max = score;
            }

            return max;
        }
    }
}
