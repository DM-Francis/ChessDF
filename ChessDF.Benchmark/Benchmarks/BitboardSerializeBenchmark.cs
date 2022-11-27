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
    public class BitboardSerializeBenchmark
    {
        private static readonly Random Rng = new(2809);
        private readonly Bitboard _bitboard;

        public BitboardSerializeBenchmark()
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
        public ReadOnlySpan<int> SerializeCurrentImpl() => _bitboard.Serialize();

        [Benchmark]
        public List<int> SerializeStandardListLoop()
        {
            var output = new List<int>(_bitboard.PopCount());

            if (_bitboard.IsEmpty)
                return output;

            ulong x = _bitboard;
            while (x > 0)
            {
                ulong firstBit = x & 0 - x;
                int bitIndex = BitUtils.BitScanForward(firstBit);
                output.Add(bitIndex);

                x &= x - 1;
            }

            return output;
        }

        [Benchmark]
        public int[] SerializeStandardArrayLoop()
        {
            var output = new int[_bitboard.PopCount()];

            if (_bitboard.IsEmpty)
                return output;

            int i = 0;
            ulong x = _bitboard;
            while (x > 0)
            {
                ulong firstBit = x & 0 - x;
                int bitIndex = BitUtils.BitScanForward(firstBit);
                output[i++] = bitIndex;

                x &= x - 1;
            }

            return output;
        }

        [Benchmark]
        public List<int> SerializeByFirstGettingIndividualBits()
        {
            var output = new List<int>(_bitboard.PopCount());

            if (_bitboard.IsEmpty)
                return output;

            Bitboard[] list = _bitboard.IndividualBits();
            for (int i = 0; i < list.Length; i++)
            {
                Bitboard bit = list[i];
                int bitIndex = BitUtils.BitScanForward(bit);
                output.Add(bitIndex);
            }

            return output;
        }
    }
}
