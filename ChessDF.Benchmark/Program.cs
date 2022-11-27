using BenchmarkDotNet.Running;
using ChessDF.Benchmark.Benchmarks;
using System;

namespace ChessDF.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<BitboardSerializeBenchmark>();
            BenchmarkRunner.Run<BitboardLoopBenchmark>();
            //BenchmarkRunner.Run<MoveTypeBenchmark>();
            //BenchmarkRunner.Run<EnumCastBenchmark>();

            //BenchmarkRunner.Run<MoveGeneratorBenchmarks>();
        }
    }
}
