using ChessDF.Core;
using ChessDF.Evaluation;
using ChessDF.Moves;
using ChessDF.Searching;
using ChessDF.Uci.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessDF.Uci
{
    public class UciListener : IOutput
    {
        private static readonly Random _rng = new Random();
        private Position? _currentPosition;
        private Dictionary<ulong, Node> _nodeCache = new();
        private ZobristGenerator _generator = new();

        private const int DefaultSearchDepth = 5;

        public void Run()
        {
            string? rawCommand;
            while ((rawCommand = Console.ReadLine()) is not null)
            {
                string[] commandData = rawCommand.Split(new[] { ' ', '\t' }, StringSplitOptions.TrimEntries);

                string commandName = commandData[0];
                string[] commandArgs = commandData[1..];

                if (commandName == Command.Quit)
                {
                    break;
                }
                else if (commandName == Command.Uci)
                {
                    Console.WriteLine(new IdCommand(IdCommandType.Name, "ChessDF"));
                    Console.WriteLine(new IdCommand(IdCommandType.Author, "David Francis"));
                    Console.WriteLine(Command.UciOk);
                }
                else if (commandName == Command.IsReady)
                {
                    Console.WriteLine("readyok");
                }
                else if (commandName == Command.UciNewGame)
                {

                }
                else if (commandName == Command.Position)
                {
                    var positionCommand = new PositionCommand(commandArgs);
                    _currentPosition = positionCommand.PositionObject;
                }
                else if (commandName == Command.Go)
                {
                    var goCommand = new GoCommand(commandArgs);

                    if (!goCommand.Infinite)
                    {
                        Move move = SearchForBestMove(goCommand.Depth ?? DefaultSearchDepth);
                        Console.WriteLine(new BestMoveCommand(move));
                    }
                }
                else if (commandName == Command.Stop)
                {
                    Move move = GetRandomMove();
                    Console.WriteLine(new BestMoveCommand(move));
                }
            }
        }

        public void WriteDebug(string text)
        {
            Console.WriteLine(new InfoCommand(text));
        }

        private Move GetRandomMove()
        {
            if (_currentPosition is null)
                throw new InvalidOperationException("Position not yet specified");

            List<Move> availableMoves = MoveGenerator.GetAllMoves(_currentPosition);
            int randomIndex = _rng.Next() % availableMoves.Count;

            return availableMoves[randomIndex];
        }

        private Move SearchForBestMove(int depth)
        {
            if (_currentPosition is null)
                throw new InvalidOperationException("Position not yet specified");

            var search = new AlphaBetaSearch(new ScoreEvalWithRandomness(), this);
            search.Search(_currentPosition, depth);

            return search.BestMove;
        }
    }
}
