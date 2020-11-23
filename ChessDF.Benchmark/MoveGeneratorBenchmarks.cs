using BenchmarkDotNet.Attributes;
using ChessDF.Core;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Benchmark
{
    public class MoveGeneratorBenchmarks
    {
        private Position _position;

        public MoveGeneratorBenchmarks()
        {
            // 'Kiwi Pete'
            var board = new Board
            {
                WhitePawns = 0x00_00_00_08_10_00_e7_00,
                WhiteKnights = 0x00_00_00_10_00_04_00_00,
                WhiteBishops = 0x00_00_00_00_00_00_18_00,
                WhiteRooks = 0x00_00_00_00_00_00_00_81,
                WhiteQueens = 0x00_00_00_00_00_20_00_00,
                WhiteKing = 0x00_00_00_00_00_00_00_10,

                BlackPawns = 0x00_2d_50_00_02_80_00_00,
                BlackKnights = 0x00_00_22_00_00_00_00_00,
                BlackBishops = 0x00_40_01_00_00_00_00_00,
                BlackRooks = 0x81_00_00_00_00_00_00_00,
                BlackQueens = 0x00_10_00_00_00_00_00_00,
                BlackKing = 0x10_00_00_00_00_00_00_00
            };

            _position = new Position(board, Side.White, null, CastlingRights.All, 0);
        }

        [Benchmark]
        public void GetPawnMoves() => MoveGenerator.AddAllPawnMoves(_position, new List<Move>());

        [Benchmark]
        public void GetKnightMoves() => MoveGenerator.AddKnightMoves(_position, new List<Move>());

        [Benchmark]
        public void GetBishopMoves() => MoveGenerator.AddBishopMoves(_position, new List<Move>());

        [Benchmark]
        public void GetRookMoves() => MoveGenerator.AddRookMoves(_position, new List<Move>());

        [Benchmark]
        public void GetQueenMoves() => MoveGenerator.AddQueenMoves(_position, new List<Move>());

        [Benchmark]
        public void GetKingMoves() => MoveGenerator.AddKingMoves(_position, new List<Move>());

        [Benchmark]
        public void GetCastlingMoves() => MoveGenerator.AddCastlingMoves(_position, new List<Move>());
    }
}
