using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Benchmark.Benchmarks
{
    public class EnumCastBenchmark
    {
        public enum EnumInt
        {
            a, b, c, d, e
        }

        public enum EnumUInt : uint
        {
            a, b, c, d, e
        }

        private static readonly Random _rng = new Random();
        private EnumInt _enumInt;
        private EnumUInt _enumUInt;

        public EnumCastBenchmark()
        {

            _enumInt = (EnumInt)CreateRandomUint();
            _enumUInt = (EnumUInt)CreateRandomUint();
        }

        private static uint CreateRandomUint()
        {
            byte[] bytes = new byte[4];
            _rng.NextBytes(bytes);

            uint finalBits = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                finalBits |= (uint)bytes[i] << i * bytes.Length;
            }

            return finalBits;
        }

        [Benchmark]
        public uint EnumIntToUint() => (uint)_enumInt;

        [Benchmark]
        public uint EnumUIntToUint() => (uint)_enumUInt;
    }
}
