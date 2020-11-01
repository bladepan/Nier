using BenchmarkDotNet.Running;

namespace Nier.TwoB.Benchmarks
{
    class Program
    {
        public static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run(typeof(CharSequenceHashCodeBenchmarks));
            _ = BenchmarkRunner.Run(typeof(CharSequenceEqualsBenchmarks));
            _ = BenchmarkRunner.Run(typeof(CharSequenceDictionaryBenchmarks));
        }
    }
}
