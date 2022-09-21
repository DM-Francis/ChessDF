using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessDF.Moves;

namespace ChessDF.Uci
{
    interface IOutput
    {
        void WriteDebug(string text);
        void WriteCurrentMoveInfo(Move currentMove, int currentMoveNumber);
        void WriteBestLineInfo(int depth, double score, int nodes, IEnumerable<Move> bestLine);
    }
}
