using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Uci.Commands
{
    internal abstract class Command
    {
        public const string Uci = "uci";
        public const string UciOk = "uciok";
        public const string IsReady = "isready";
        public const string Quit = "quit";
        public const string UciNewGame = "ucinewgame";
        public const string Position = "position";
        public const string Go = "go";
        public const string Stop = "stop";

        public abstract string CommandName { get; }

        public override string ToString()
        {
            string commandTypeName = GetType().Name;
            string command = commandTypeName.Replace("Command", "").ToLower();

            return command;
        }
    }
}
