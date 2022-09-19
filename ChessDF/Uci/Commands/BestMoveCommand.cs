using ChessDF.Core;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Uci.Commands
{
    class BestMoveCommand : Command
    {
        public override string CommandName => "bestmove";

        public string MoveString { get; }

        public BestMoveCommand(Move move)
        {
            MoveString = move.ToUciMoveString();
        }

        public override string ToString()
        {
            return $"bestmove {MoveString}";
        }
    }
}
