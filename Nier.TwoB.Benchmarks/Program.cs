using BenchmarkDotNet.Running;

namespace Nier.TwoB.Benchmarks
{
    class Program
    {
        public static void Main(string[] args)
        {
            // _ = BenchmarkRunner.Run(typeof(TwoBStringHashCodeBenchmarks));
            _ = BenchmarkRunner.Run(typeof(TwoBStringEqualsBenchmarks));
        }
    }
}
