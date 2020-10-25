using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Nier.TwoB.Benchmarks
{
    public class TwoBStringHashCodeBenchmarks
    {
        private const int StrCount = 1024;
        private const int StrLength = 1024;
        private readonly string[] _strings;
        private readonly TwoBString[] _2bStrings;

        public TwoBStringHashCodeBenchmarks()
        {
            _strings = Enumerable.Range(0, StrCount).Select(i => StringGenerator.GenString(StrLength)).ToArray();
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
    }
}
