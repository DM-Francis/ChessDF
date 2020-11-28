using ChessDF.Core;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Uci.Commands
{
    class PositionCommand : Command
    {
        public override string CommandName => "position";
        public Position PositionObject { get; }

        public PositionCommand(IEnumerable<string> args)
        {
            var argsList = args.ToList();
            int movesIndex = argsList.FindIndex(a => a == "moves");

            Position position;
            if (argsList[0] == "startpos")
                position = Core.Position.StartPosition;
            else if (argsList[0] == "fen")
            {
                int endIndex = movesIndex == -1 ? argsList.Count : movesIndex;
                string[] fenArgs = argsList.ToArray()[1..endIndex];
                string fenString = string.Join(' ', fenArgs);
                position = Core.Position.FromFENString(fenString);
            }
            else
            {
                throw new ArgumentException("Invalid position command, must specify either startpos or a fen string");
            }

            if (movesIndex != -1)
            {
                int firstMove = movesIndex + 1;
                string[] moves = argsList.ToArray()[firstMove..];
                foreach (string moveString in moves)
                {
                    Move move = Move.FromStringAndPosition(moveString, position);
                    position = position.MakeMove(move);
                }
            }

            PositionObject = position;
        }
    }
}
