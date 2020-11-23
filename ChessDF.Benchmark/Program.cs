using BenchmarkDotNet.Running;
using System;

namespace ChessDF.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            // BenchmarkRunner.Run<BitboardLoopBenchmark>();
            //BenchmarkRunner.Run<MoveTypeBenchmark>();
            //BenchmarkRunner.Run<EnumCastBenchmark>();

            BenchmarkRunner.Run<MoveGeneratorBenchmarks>();
        }
    }
}
