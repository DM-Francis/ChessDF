using BenchmarkDotNet.Running;
using System;

namespace ChessDF.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BitboardLoopBenchmark>();
        }
    }
}
