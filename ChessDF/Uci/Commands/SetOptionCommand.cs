using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Uci.Commands
{
    class SetOptionCommand : Command
    {
        public override string CommandName => "setoption";

        public string Name { get; }
        public string? Value { get; }

        public SetOptionCommand(string name, string? value = null)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            if (Value is null)
                return $"{CommandName} name {Name}";
            else
                return $"{CommandName} name {Name} value {Value}";
        }
    }
}
