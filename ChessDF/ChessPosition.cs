using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDF
{
    public class ChessPosition
    {
        private const int _boardSize = 8;
        private readonly char[,] _chessGrid = new char[_boardSize, _boardSize];     // row, column

        private static readonly char[] _pieceChars = new char[] { 'k', 'q', 'r', 'b', 'n', 'p', 'K', 'Q', 'R', 'B', 'N', 'P' };

        public ChessPosition(string? fenString = null)
        {
            if (fenString is null)
            {
                SetupBoardFromFen(FEN.StartingPosition);
            }
            else
            {
                SetupBoardFromFen(new FEN(fenString));
            }
        }

        public char this[int row, int column] => _chessGrid[row, column];

        private void SetupBoardFromFen(FEN fen)
        {
            string piecePlacement = fen.PiecePlacement;

            int row = 0;
            int column = 0;

            for (int i = 0; i < piecePlacement.Length; i++)
            {
                if (_pieceChars.Any(f => f == piecePlacement[i]))
                {
                    _chessGrid[row, column] = piecePlacement[i];
                    column++;
                }
                else if (piecePlacement[i] == '/')
                {
                    if (column != 8)
                        throw new ArgumentException("FEN is invalid - too many pieces on a single row.");

                    row++;
                    column = 0;
                }
                else if (int.TryParse(piecePlacement[i].ToString(), out int emptySquares))
                {
                    for (int e = 0; e < emptySquares; e++)
                    {
                        _chessGrid[row, column] = ' ';
                        column++;

                        if (column > 8)
                            throw new ArgumentException("FEN is invalid - too many empty squares on a single row.");
                    }
                }
                else
                {
                    throw new ArgumentException($"FEN is invalid - found invalid charater: {piecePlacement[i]}");
                }
            }
        }
    }
}
