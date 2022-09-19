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
    public interface ISearchStrategy
    {
        ReadOnlyCollection<SearchResult> MoveScores { get; }
        ReadOnlyCollection<SearchResult> Search(Position position, int depth, CancellationToken cancelToken = default);
    }
}
