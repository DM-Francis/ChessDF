using ChessDF.Core;
using ChessDF.Uci;
using System;

namespace ChessDF
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new UciListener();

            listener.Run();
        }
    }
}
