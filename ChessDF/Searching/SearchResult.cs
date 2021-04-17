using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Searching
{
    public class SearchResult
    {
        public SearchResult(Move move, double score, int ordering)
        {
            Move = move;
            Score = score;
            Ordering = ordering;
        }

        public Move Move { get; }
        public double Score { get; }
        public int Ordering { get; }
    }
}
