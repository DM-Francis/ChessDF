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
    public class UciListener
    {
        private static readonly Random _rng = new Random();
        private Position? _currentPosition;

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
                else if (commandName == Command.UciNewGame) { }
                else if (commandName == Command.Position)
                {
                    PositionCommand positionCommand;

                    if (commandArgs.Length == 1)
                        positionCommand = new PositionCommand(commandArgs[0], null);
                    else
                        positionCommand = new PositionCommand(commandArgs[0], commandArgs[2..]);

                    _currentPosition = positionCommand.PositionObject;
                }
                else if (commandName == Command.Go)
                {
                    var goCommand = new GoCommand(commandArgs);

                    if (!goCommand.Infinite)
                    {
                        Move move = SearchForBestMove();
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

        private Move GetRandomMove()
        {
            if (_currentPosition is null)
                throw new InvalidOperationException("Position not yet specified");

            List<Move> availableMoves = MoveGenerator.GetAllMoves(_currentPosition);
            int randomIndex = _rng.Next() % availableMoves.Count;

            return availableMoves[randomIndex];
        }

        private Move SearchForBestMove()
        {
            if (_currentPosition is null)
                throw new InvalidOperationException("Position not yet specified");

            var search = new Search(new BasicScoreEvaluation());
            return search.RootNegaMax(_currentPosition, 3);
        }
    }
}
