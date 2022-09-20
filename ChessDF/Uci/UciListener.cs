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

        private const int DefaultMaxSearchDepth = 10;

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
                        Move move = SearchForBestMove(goCommand);
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

        public void WriteCurrentMoveInfo(Move currentMove, int currentMoveNumber)
        {
            Console.WriteLine($"info currmove {currentMove.ToUciMoveString()} currmovenumber {currentMoveNumber}");
        }

        public void WriteBestLineInfo(int depth, double score, int nodes, Move[] bestLine)
        {
            var bestLineMoveStrings = bestLine.Select(m => m.ToUciMoveString());
            var scoreCentipawns = score * 100;
            Console.WriteLine($"info depth {depth} score cp {Math.Round(scoreCentipawns)} nodes {nodes} pv {string.Join(' ', bestLineMoveStrings)}");
        }

        public void WriteInfo(Move currentMove, int currentMoveNumber, int nodes, double score)
        {
            Console.WriteLine($"info currmove {currentMove.ToUciMoveString()} nodes {nodes} score cp {score * 100}" );
        }

        private Move GetRandomMove()
        {
            if (_currentPosition is null)
                throw new InvalidOperationException("Position not yet specified");

            List<Move> availableMoves = MoveGenerator.GetAllMoves(_currentPosition);
            int randomIndex = _rng.Next() % availableMoves.Count;

            return availableMoves[randomIndex];
        }

        private Move SearchForBestMove(GoCommand goCommand)
        {
            if (_currentPosition is null)
                throw new InvalidOperationException("Position not yet specified");

            int? time = _currentPosition.SideToMove == Side.White ? goCommand.WTime : goCommand.BTime;

            var search = new IterativeDeepeningSearch(new BasicScoreEvaluation(), this);
            CancellationToken cancelToken = default;
            if (time is not null)
            {
                int timeForCurrentMove = time.Value / 20;
                var tokenSource = new CancellationTokenSource(timeForCurrentMove);
                cancelToken = tokenSource.Token;
            }

            search.Search(_currentPosition, goCommand.Depth ?? DefaultMaxSearchDepth, cancelToken);
            return search.BestMove;
        }
    }
}
