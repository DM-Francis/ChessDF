using ChessDF.Core;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessDF.Searching
{
    internal class IterativeDeepeningSearch
    {
        private readonly ISearchStrategy _searchStrategy;

        private List<SearchResult> _moveScores = new();
        public ReadOnlyCollection<SearchResult> MoveScores => _moveScores.AsReadOnly();

        public IterativeDeepeningSearch(ISearchStrategy searchStrategy)
        {
            _searchStrategy = searchStrategy;
        }

        public Task Start(Position position, int maxDepth, CancellationToken cancelToken)
        {
            return Task.Run(() => RunIterativeSearch(position, maxDepth, cancelToken), cancelToken);
        }

        private void RunIterativeSearch(Position position, int maxDepth, CancellationToken cancelToken)
        {
            var moves = MoveGenerator.GetAllMoves(position);
            for (int depth = 1; depth <= maxDepth; depth++)
            {
                _moveScores = _searchStrategy.Search(position, depth, moves, cancelToken).ToList();
                moves = _moveScores
                            .OrderByDescending(ms => ms.Score)
                            .ThenBy(ms => ms.Ordering)
                            .Select(ms => ms.Move).ToList();
            }
        }
    }
}
