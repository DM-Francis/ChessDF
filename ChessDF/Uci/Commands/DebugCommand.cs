using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Uci.Commands
{
    internal class DebugCommand : Command
    {
        public override string CommandName => "debug";
        public DebugValue Value { get; }

        public DebugCommand(DebugValue value) => Value = value;

        public override string ToString() => $"{CommandName} {Value.ToString().ToLower()}";
    }

    public enum DebugValue
    {
        On, Off
    }
}
