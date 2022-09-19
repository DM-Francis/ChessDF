using System.Collections.Generic;
using System.Threading;
using ChessDF.Core;
using ChessDF.Moves;

namespace ChessDF.Searching;

public interface ISearch
{
    void Search(Position position, int maxDepth, CancellationToken cancellationToken = default);
    Move BestMove { get; }
}