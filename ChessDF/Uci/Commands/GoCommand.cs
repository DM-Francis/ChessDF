using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Uci.Commands
{
    class GoCommand : Command
    {
        public override string CommandName => "go";

        public ReadOnlyCollection<string> SearchMoves { get; }
        public bool Ponder { get; init; }
        public int? WTime { get; init; }
        public int? BTime { get; init; }
        public int? WInc { get; init; }
        public int? BInc { get; init; }
        public int? MovesToGo { get; init; }
        public int? Depth { get; init; }
        public int? Nodes { get; init; }
        public int? Mate { get; init; }
        public int? Movetime { get; init; }
        public bool Infinite { get; init; }

        public GoCommand(params string[] args)
        {
            var argsList = new List<string>(args);

            int searchMovesIndex = argsList.FindIndex(s => s == "searchmoves");
            int ponderIndex = argsList.FindIndex(s => s == "ponder");
            int wtimeIndex = argsList.FindIndex(s => s == "wtime");
            int btimeIndex = argsList.FindIndex(s => s == "btime");
            int wincIndex = argsList.FindIndex(s => s == "winc");
            int bincIndex = argsList.FindIndex(s => s == "binc");
            int movesToGoIndex = argsList.FindIndex(s => s == "movestogo");
            int depthIndex = argsList.FindIndex(s => s == "depth");
            int nodesIndex = argsList.FindIndex(s => s == "nodes");
            int mateIndex = argsList.FindIndex(s => s == "mate");
            int movetimeIndex = argsList.FindIndex(s => s == "movetime");
            int infiniteIndex = argsList.FindIndex(s => s == "infinite");

            if (searchMovesIndex != -1)
                SearchMoves = new List<string>(args[(searchMovesIndex + 1)..]).AsReadOnly();
            else
                SearchMoves = new List<string>().AsReadOnly();

            Ponder = ponderIndex != -1;
            Infinite = infiniteIndex != -1;

            if (wtimeIndex != -1)
                WTime = int.Parse(args[wtimeIndex + 1]);
            if (btimeIndex != -1)
                BTime = int.Parse(args[btimeIndex + 1]);
            if (wincIndex != -1)
                WInc = int.Parse(args[wincIndex + 1]);
            if (bincIndex != -1)
                BInc = int.Parse(args[bincIndex + 1]);
            if (movesToGoIndex != -1)
                MovesToGo = int.Parse(args[movesToGoIndex + 1]);
            if (depthIndex != -1)
                Depth = int.Parse(args[depthIndex + 1]);
            if (nodesIndex != -1)
                Nodes = int.Parse(args[nodesIndex + 1]);
            if (mateIndex != -1)
                Mate = int.Parse(args[mateIndex + 1]);
            if (movetimeIndex != -1)
                Movetime = int.Parse(args[movetimeIndex + 1]);
        }

        public GoCommand(IEnumerable<string>? searchMoves = null)
        {
            if (searchMoves is null)
                SearchMoves = new List<string>().AsReadOnly();
            else
                SearchMoves = new List<string>(searchMoves).AsReadOnly();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder(CommandName);

            if (SearchMoves.Count != 0)
            {
                string allMoves = SearchMoves.Aggregate((all, move) => all += $" {move}");
                stringBuilder.Append($" searchmoves {allMoves}");
            }

            if (Ponder)
                stringBuilder.Append(" ponder");
            if (WTime is not null)
                stringBuilder.Append($" wtime {WTime}");
            if (BTime is not null)
                stringBuilder.Append($" btime {BTime}");
            if (WInc is not null)
                stringBuilder.Append($" winc {WInc}");
            if (BInc is not null)
                stringBuilder.Append($" binc {BInc}");
            if (MovesToGo is not null)
                stringBuilder.Append($" movestogo {MovesToGo}");
            if (Depth is not null)
                stringBuilder.Append($" depth {Depth}");
            if (Nodes is not null)
                stringBuilder.Append($" nodes {Nodes}");
            if (Mate is not null)
                stringBuilder.Append($" mate {Mate}");
            if (Movetime is not null)
                stringBuilder.Append($" movetime {Movetime}");
            if (Infinite)
                stringBuilder.Append($" infinite");

            return stringBuilder.ToString();
        }
    }
}
