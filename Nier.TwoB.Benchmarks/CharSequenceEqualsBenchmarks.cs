using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Nier.TwoB.Benchmarks
{
    public class CharSequenceEqualsBenchmarks
    {
        private const int StrCount = 1024;
        private const int StrLength = 1024;
        private readonly string[] _strings;
        // has same content as _strings
        private readonly string[] _strings2;
        // same prefix different content
        private readonly string[] _strings3;

        private readonly CharSequence[] _2bStrings;
        private readonly CharSequence[] _2bStrings2;
        private readonly CharSequence[] _2bStrings3;

        public CharSequenceEqualsBenchmarks()
        {
            _strings = Enumerable.Range(0, StrCount).Select(i => StringGenerator.GenString(StrLength - 4) + "abcd").ToArray();
            _2bStrings = _strings.Select(s => CharSequence.FromValue(s)).ToArray();
            _strings3 = _strings.Select(s => s.Substring(0, StrLength - 4) + "1234").ToArray();

            _strings2 = _strings.Select(s => new string(s)).ToArray();
            _2bStrings2 = _strings2.Select(s => CharSequence.FromValue(s)).ToArray();
            _2bStrings3 = _strings3.Select(s => CharSequence.FromValue(s)).ToArray();
        }

        [Benchmark]
        public int StringEqualsFalse()
        {
            int equalsCount = 0;
            for (int i = 0; i < _strings.Length - 1; i++)
            {
                bool equals = _strings[i].Equals(_strings[i + 1]);
                if (equals)
                {
                    equalsCount++;
                }
            }
            return equalsCount;
        }

        [Benchmark]
        public int TwoBStringEqualsFalse()
        {
            int equalsCount = 0;
            for (int i = 0; i < _2bStrings.Length - 1; i++)
            {
                bool equals = _2bStrings[i].Equals(_2bStrings[i + 1]);
                if (equals)
                {
                    equalsCount++;
                }
            }
            return equalsCount;
        }

        [Benchmark]
        public int StringEqualsSharePrefix()
        {
            int equalsCount = 0;
            for (int i = 0; i < _strings.Length; i++)
            {
                bool equals = _strings[i].Equals(_strings3[i]);
                if (equals)
                {
                    equalsCount++;
                }
            }
            return equalsCount;
        }

        [Benchmark]
        public int TwoBStringEqualsSharePrefix()
        {
            int equalsCount = 0;
            for (int i = 0; i < _2bStrings.Length; i++)
            {
                bool equals = _2bStrings[i].Equals(_2bStrings3[i]);
                if (equals)
                {
                    equalsCount++;
                }
            }
            return equalsCount;
        }

        [Benchmark]
        public int StringEqualsTrue()
        {
            int equalsCount = 0;
            for (int i = 0; i < _strings.Length; i++)
            {
                bool equals = _strings[i].Equals(_strings2[i]);
                if (equals)
                {
                    equalsCount++;
                }
            }
            return equalsCount;
        }

        [Benchmark]
        public int TwoBStringEqualsTrue()
        {
            int equalsCount = 0;
            for (int i = 0; i < _2bStrings.Length; i++)
            {
                bool equals = _2bStrings[i].Equals(_2bStrings2[i]);
                if (equals)
                {
                    equalsCount++;
                }
            }

            return equalsCount;
        }
    }
}
