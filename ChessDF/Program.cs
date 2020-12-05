using ChessDF.Core;
using ChessDF.Magics;
using ChessDF.Uci;
using System;
using System.IO;
using System.Linq;

namespace ChessDF
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("--generateMagics"))
            {
                string targetFile = Path.Combine(Environment.CurrentDirectory, "magics.txt");
                MagicGenerator.GenerateMagicsToFile(targetFile);
                return;
            }

            var listener = new UciListener();

            listener.Run();
        }
    }
}
