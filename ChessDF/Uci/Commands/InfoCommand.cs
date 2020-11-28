using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Uci.Commands
{
    class InfoCommand
    {
        public string Text { get; }

        public InfoCommand(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return $"info string {Text}";
        }
    }
}
