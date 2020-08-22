using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDF
{
    public class ConsoleChessPositionRenderer
    {
        private readonly Dictionary<char, string> _fenToUnicode = new Dictionary<char, string>
        {
            ['k'] = "♚",
            ['q'] = "♛",
            ['r'] = "♜",
            ['b'] = "♝",
            ['n'] = "♞",
            ['p'] = "♟︎",
            ['K'] = "♔",
            ['Q'] = "♕",
            ['R'] = "♖",
            ['B'] = "♗",
            ['N'] = "♘",
            ['P'] = "♙",
            [' '] = " "
        };

        public void RenderUnicode(ChessPosition position)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Console.Write(_fenToUnicode[position[row, col]]);
                }

                Console.WriteLine();
            }
        }

        public void RenderBasic(ChessPosition position)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Console.Write(position[row, col]);
                }

                Console.WriteLine();
            }
        }
    }
}
