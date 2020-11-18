using ChessDF.Core;
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

        public void RenderBBBoard(Board board)
        {
            char[,] allPieces = new char[8, 8];

            AddPiecesToPosition(board.WhitePawns, 'P', allPieces);
            AddPiecesToPosition(board.WhiteKnights, 'N', allPieces);
            AddPiecesToPosition(board.WhiteBishops, 'B', allPieces);
            AddPiecesToPosition(board.WhiteRooks, 'R', allPieces);
            AddPiecesToPosition(board.WhiteQueens, 'Q', allPieces);
            AddPiecesToPosition(board.WhiteKing, 'K', allPieces);
            AddPiecesToPosition(board.BlackPawns, 'p', allPieces);
            AddPiecesToPosition(board.BlackKnights, 'n', allPieces);
            AddPiecesToPosition(board.BlackBishops, 'b', allPieces);
            AddPiecesToPosition(board.BlackRooks, 'r', allPieces);
            AddPiecesToPosition(board.BlackQueens, 'q', allPieces);
            AddPiecesToPosition(board.BlackKing, 'k', allPieces);

            for (int row = 7; row >= 0; row--)
            {
                for (int col = 0; col < 8; col++)
                {
                    Console.Write(allPieces[row, col]);
                }

                Console.WriteLine();
            }
        }

        private static void AddPiecesToPosition(Bitboard bitboard, char pieceChar, char[,] allPieces)
        {
            List<int> pieceIndices = bitboard.Serialize();
            foreach(int index in pieceIndices)
            {
                (int row, int col) p = IndexToRowAndCol(index);
                allPieces[p.row, p.col] = pieceChar;
            }
        }

        private static (int row, int col) IndexToRowAndCol(int index) => (index / 8, index % 8);
    }
}
