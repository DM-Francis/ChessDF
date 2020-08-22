using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ChessDF
{
    public class ChessPosition
    {
        private const int _boardSize = 8;
        private readonly char[,] _chessGrid = new char[_boardSize, _boardSize];     // row, column

        private static readonly char[] _pieceChars = new char[] { 'k', 'q', 'r', 'b', 'n', 'p', 'K', 'Q', 'R', 'B', 'N', 'P' };
        private static readonly Dictionary<char, int> _fileToColumn = new Dictionary<char, int>
        {
            ['a'] = 0,
            ['b'] = 1,
            ['c'] = 2,
            ['d'] = 3,
            ['e'] = 4,
            ['f'] = 5,
            ['g'] = 6,
            ['h'] = 7
        };
        private static readonly Dictionary<int, char> _columnToFile = _fileToColumn.ToDictionary(kv => kv.Value, kv => kv.Key);

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
        public char this[string coordinate]
        {
            get
            {
                ValidateCoordinate(coordinate);
                (int row, int column) = ConvertCoordinateToGrid(coordinate);

                return _chessGrid[row, column];
            }
        }

        private void ValidateCoordinate(string coordinate)
        {
            if (coordinate.Length != 2)
                throw new ArgumentOutOfRangeException();

            if (!_fileToColumn.ContainsKey(coordinate[0]))
                throw new ArgumentOutOfRangeException();

            if (!int.TryParse(coordinate[1].ToString(), out int rank))
                throw new ArgumentOutOfRangeException();

            if (rank < 1 || rank > 8)
                throw new ArgumentOutOfRangeException();
        }

        private (int row, int col) ConvertCoordinateToGrid(string coordinate)
        {
            int row = 8 - int.Parse(coordinate[1].ToString());
            int column = _fileToColumn[coordinate[0]];

            return (row, column);
        }

        private string ConvertGridToCoordinate(int row, int col)
        {
            int rank = 8 - row;
            char file = _columnToFile[col];

            return string.Concat(file, rank);
        }

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
