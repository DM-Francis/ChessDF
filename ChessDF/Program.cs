using System;

namespace ChessDF
{
    class Program
    {
        static void Main(string[] args)
        {
            var renderer = new ConsoleChessBoardRenderer();

            var positions = new[]
            {
                new ChessBoard(),
                new ChessBoard("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1"),
                new ChessBoard("rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR w KQkq c6 0 2"),
                new ChessBoard("rnbqkbnr/pp1ppppp/8/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2"),
                new ChessBoard("4k3/8/8/8/8/8/4P3/4K3 w - - 5 39"),
            };

            foreach(var pos in positions)
            {
                renderer.RenderBasic(pos);
                Console.WriteLine();
            }
        }
    }
}
