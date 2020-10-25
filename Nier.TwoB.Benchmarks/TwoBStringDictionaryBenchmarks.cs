using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Nier.TwoB.Benchmarks
{
    public class TwoBStringDictionaryBenchmarks
    {
        private const int StrCount = 1024;
        private const int StrLength = 1024;

        private readonly string[] _strings;

        // has same content as _strings
        private readonly string[] _strings2;
        private readonly Dictionary<string, int> _strDict = new Dictionary<string, int>();

        private readonly TwoBString[] _2bStrings;
        private readonly TwoBString[] _2bStrings2;
        private readonly Dictionary<TwoBString, int> _2bDict = new Dictionary<TwoBString, int>();

        public TwoBStringDictionaryBenchmarks()
        {
            _strings = Enumerable.Range(0, StrCount).Select(i => StringGenerator.GenString(StrLength)).ToArray();
            _2bStrings = _strings.Select(s => TwoBString.FromValue(s)).ToArray();
            foreach (string s in _strings)
            {
                _strDict[s] = 1;
            }

            foreach (TwoBString s in _2bStrings)
            {
                _2bDict[s] = 1;
            }

            _strings2 = _strings.Select(s => new string(s)).ToArray();
            _2bStrings2 = _strings2.Select(s => TwoBString.FromValue(s)).ToArray();
        }

        [Benchmark]
        public int DictGet()
        {
            int result = 0;
            foreach (string s in _strings2)
            {
                result += _strDict[s];
            }

            return result;
        }

        [Benchmark]
        public int TwoBDictGet()
        {
            int result = 0;
            foreach (TwoBString s in _2bStrings2)
            {
                result += _2bDict[s];
            }

            return result;
        }
    }
}
