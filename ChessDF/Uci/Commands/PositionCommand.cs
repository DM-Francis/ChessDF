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

        public string PositionString { get; }
        public ReadOnlyCollection<string> MoveStrings { get; } = new List<string>().AsReadOnly();

        public Position PositionObject { get; }

        public PositionCommand(string positionString, IEnumerable<string>? moves)
        {
            PositionString = positionString;

            Position position;
            if (positionString == "startpos")
                position = Core.Position.StartPosition;
            else
                position = Core.Position.FromFENString(positionString);

            if (moves is not null)
            {
                MoveStrings = new List<string>(moves).AsReadOnly();
                foreach (string moveString in moves)
                {
                    Move move = Move.FromStringAndPosition(moveString, position);
                    position = position.MakeMove(move);
                }
            }

            PositionObject = position;
        }

        public override string ToString()
        {
            if (MoveStrings.Count == 0)
            {
                return $"{CommandName} {PositionString}";
            }
            else
            {
                string allMoves = MoveStrings.Aggregate((all, move) => all += $" {move}");
                return $"{CommandName} {PositionString} moves {allMoves}";
            }
        }
    }
}
