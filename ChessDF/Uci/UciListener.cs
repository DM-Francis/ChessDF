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
        private ISearch? _search;
        private CancellationTokenSource? _cancellationTokenSource = new CancellationTokenSource();

        private const int DefaultMaxSearchDepth = 10;

        private Task? _currentSearch;

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
                        KickOffSearch(goCommand);
                    }
                }
                else if (commandName == Command.Stop)
                {
                    _cancellationTokenSource?.Cancel();
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

        public void WriteBestLineInfo(int depth, double score, int nodes, IEnumerable<Move> bestLine)
        {
            var bestLineMoveStrings = bestLine.Select(m => m.ToUciMoveString());
            var scoreCentipawns = score * 100;
            Console.WriteLine($"info depth {depth} score cp {Math.Round(scoreCentipawns)} nodes {nodes} pv {string.Join(' ', bestLineMoveStrings)}");
        }

        public void WriteInfo(Move currentMove, int currentMoveNumber, int nodes, double score)
        {
            Console.WriteLine($"info currmove {currentMove.ToUciMoveString()} nodes {nodes} score cp {score * 100}" );
        }

        private void KickOffSearch(GoCommand goCommand)
        {
            if (_currentPosition is null)
                throw new InvalidOperationException("Position not yet specified");

            int? time = _currentPosition.SideToMove == Side.White ? goCommand.WTime : goCommand.BTime;

            _search = new IterativeDeepeningSearch(new PieceTableEvaluation(), this);
            if (time is not null)
            {
                int timeForCurrentMove = Math.Min(time.Value / 20, 10000);
                _cancellationTokenSource = new CancellationTokenSource(timeForCurrentMove);
            }
            else
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }

            CancellationToken cancelToken = _cancellationTokenSource.Token;
            Task.Run(() =>
            {
                _search.Search(_currentPosition, goCommand.Depth ?? DefaultMaxSearchDepth, cancelToken);
                var move = _search.BestMove;
                Console.WriteLine(new BestMoveCommand(move));
            }, cancelToken);
        }
    }
}
