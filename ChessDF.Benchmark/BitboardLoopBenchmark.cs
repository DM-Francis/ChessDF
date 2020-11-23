using BenchmarkDotNet.Attributes;
using ChessDF.Core;
using ChessDF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDF.Benchmark
{
    [IterationTime(250)]
    [MemoryDiagnoser]
    public class BitboardLoopBenchmark
    {
        private static readonly Random _rng = new Random();
        private readonly Bitboard _bitboard;

        public BitboardLoopBenchmark()
        {
            _bitboard = CreateRandomBitboard();
        }

        private static Bitboard CreateRandomBitboard()
        {
            byte[] bytes = new byte[8];
            _rng.NextBytes(bytes);

            ulong finalBits = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                finalBits |= (ulong)bytes[i] << (i * bytes.Length);
            }

            return new Bitboard(finalBits);
        }


        [Benchmark]
        public int[] SerializeCurrentImpl() => _bitboard.Serialize();

        [Benchmark]
        public List<int> SerializeStandardListLoop()
        {
            var output = new List<int>(_bitboard.PopCount());

            if (_bitboard.IsEmpty)
                return output;

            ulong x = _bitboard;
            while (x > 0)
            {
                ulong firstBit = x & (0 - x);
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
                ulong firstBit = x & (0 - x);
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

        [Benchmark]
        public Bitboard[] IndividualBitsCurrentImpl() => _bitboard.IndividualBits();

        [Benchmark]
        public List<Bitboard> IndividualBitsStandardList()
        {
            var output = new List<Bitboard>(_bitboard.PopCount());
            for (ulong x = _bitboard; x > 0; x &= x - 1)
            {
                output.Add(x & (0 - x));
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
                output[i++] = x & (0 - x);
            }

            return output;
        }

        [Benchmark]
        public List<Bitboard> IndividualBitsIteratorList() => IndividualBitsIterator().ToList();

        public IEnumerable<Bitboard> IndividualBitsIterator()
        {
            for (ulong x = _bitboard; x > 0; x &= x - 1)
            {
                yield return x & (0 - x);
            }
        }
    }
}
