using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Nier.TwoB.Benchmarks
{
    public class TwoBStringBenchmarks
    {
        private const int StrCount = 1024;
        private const int StrLength = 1024;
        private readonly Random _random = new Random(42);
        private readonly string[] _strings;
        private readonly TwoBString[] _2bStrings;

        public TwoBStringBenchmarks()
        {
            _strings = Enumerable.Range(0, StrCount).Select(i => GenString(StrLength)).ToArray();
            _2bStrings = _strings.Select(s => TwoBString.FromValue(s)).ToArray();
        }

        [Benchmark]
        public int StringHashCode()
        {
            int result = 0;
            foreach (string s in _strings)
            {
                result += s.GetHashCode(StringComparison.Ordinal);
            }

            return result;
        }

        [Benchmark]
        public int TwoBStringHashCode()
        {
            int result = 0;
            foreach (TwoBString s in _2bStrings)
            {
                result += s.GetHashCode();
            }

            return result;
        }

        private string GenString(int length)
        {
            byte[] bytes = new byte[length];
            _random.NextBytes(bytes);
            string str = Convert.ToBase64String(bytes);
            return str.Substring(0, length);
        }
    }
}
