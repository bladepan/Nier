using System;
using BenchmarkDotNet.Running;

namespace Nier.TwoB.Benchmarks
{
    class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
