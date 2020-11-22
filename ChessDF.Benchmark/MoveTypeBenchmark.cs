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
    public class MoveTypeBenchmark
    {
        public class MoveUShort
        {
            private readonly ushort _moveData;

            public MoveUShort(Square from, Square to, MoveFlags flags)
            {
                _moveData = (ushort)(((uint)flags & 0xf) << 12 | ((uint)from & 0x3f) << 6 | ((uint)to & 0x3f));
            }
        }

        public class MoveUInt
        {
            private readonly uint _moveData;

            public MoveUInt(Square from, Square to, MoveFlags flags)
            {
                _moveData = ((uint)flags & 0xf) << 12 | ((uint)from & 0x3f) << 6 | ((uint)to & 0x3f);
            }
        }

        [Benchmark]
        public MoveUShort ShortConstructor() => new MoveUShort(Square.a1, Square.h8, MoveFlags.QuietMove);

        [Benchmark]
        public MoveUInt IntConstructor() => new MoveUInt(Square.a1, Square.h8, MoveFlags.QuietMove);
    }
}
