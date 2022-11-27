using BenchmarkDotNet.Attributes;
using ChessDF.Core;
using System;

namespace ChessDF.Benchmark.Benchmarks
{
    [IterationTime(250)]
    [MemoryDiagnoser]
    public class BitboardLoopBenchmark
    {
        private static readonly Random Rng = new(2809);
        private readonly int[] _intArray;
        private readonly Bitboard _bitboard;

        public BitboardLoopBenchmark()
        {
            _bitboard = CreateRandomBitboard();
            _intArray = _bitboard.Serialize().ToArray();
        }

        private static Bitboard CreateRandomBitboard()
        {
            byte[] bytes = new byte[8];
            Rng.NextBytes(bytes);
            ulong finalBits = BitConverter.ToUInt64(bytes);

            return new Bitboard(finalBits);
        }

        [Benchmark]
        public void ForLoopThroughArray()
        {
            int sum = 0;
            for (int i = 0; i < _intArray.Length; i++)
            {
                sum += _intArray[i];
            }
        }

        [Benchmark]
        public void ForLoopThroughSpan()
        {
            int sum = 0;
            var span = _intArray.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                sum += span[i];
            }
        }
    }
}
