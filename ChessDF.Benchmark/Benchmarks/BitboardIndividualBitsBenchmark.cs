using BenchmarkDotNet.Attributes;
using ChessDF.Core;
using ChessDF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDF.Benchmark.Benchmarks
{
    [IterationTime(250)]
    [MemoryDiagnoser]
    public class BitboardIndividualBitsBenchmark
    {
        private static readonly Random Rng = new(2809);
        private readonly Bitboard _bitboard;

        public BitboardIndividualBitsBenchmark()
        {
            _bitboard = CreateRandomBitboard();
        }

        private static Bitboard CreateRandomBitboard()
        {
            byte[] bytes = new byte[8];
            Rng.NextBytes(bytes);
            ulong finalBits = BitConverter.ToUInt64(bytes);

            return new Bitboard(finalBits);
        }

        [Benchmark]
        public Bitboard[] IndividualBitsCurrentImpl() => _bitboard.IndividualBits();

        [Benchmark]
        public List<Bitboard> IndividualBitsStandardList()
        {
            var output = new List<Bitboard>(_bitboard.PopCount());
            for (ulong x = _bitboard; x > 0; x &= x - 1)
            {
                output.Add(x & 0 - x);
            }

            return output;
        }

        [Benchmark]
        public Bitboard[] IndividualBitsStandardArray()
        {
            int i = 0;
            var output = new Bitboard[_bitboard.PopCount()];
            for (ulong x = _bitboard; x > 0; x &= x - 1)
            {
                output[i++] = x & 0 - x;
            }

            return output;
        }

        [Benchmark]
        public List<Bitboard> IndividualBitsIteratorList() => IndividualBitsIterator().ToList();

        public IEnumerable<Bitboard> IndividualBitsIterator()
        {
            for (ulong x = _bitboard; x > 0; x &= x - 1)
            {
                yield return x & 0 - x;
            }
        }
    }
}
