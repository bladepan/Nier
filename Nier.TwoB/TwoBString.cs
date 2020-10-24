using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Nier.TwoB
{
    public class TwoBString
    {
        public static readonly TwoBString Empty = new TwoBString(string.Empty);
        public static readonly TwoBString True = new TwoBString(bool.TrueString);
        public static readonly TwoBString False = new TwoBString(bool.FalseString);

        private static IMemoryCache s_cache = new MemoryCache(new MemoryCacheOptions {SizeLimit = 64 * 1024 * 1024});

        private const int MinimumCacheSize = 16;

        private static readonly TwoBString[] Numbers =
            Enumerable.Range(0, 128).Select(i => new TwoBString(i.ToString("D", CultureInfo.InvariantCulture)))
                .ToArray();

        private readonly string _val;
        private readonly int _hashCode;

        private TwoBString(string val)
        {
            _val = val;
#if NETSTANDARD2_1
            _hashCode = val.GetHashCode(StringComparison.Ordinal);
#else
            _hashCode = val.GetHashCode();
#endif
        }

        public override int GetHashCode() => _hashCode;

        public override bool Equals(object obj)
        {
            if (obj is TwoBString another)
            {
                if (obj == this)
                {
                    return true;
                }

                if (another._hashCode != _hashCode)
                {
                    return false;
                }

                return _val == another._val;
            }

            return false;
        }

        public override string ToString() => _val;

        public static TwoBString FromValue(byte val)
        {
            if (val < Numbers.Length)
            {
                return Numbers[val];
            }

            return FromValue(val.ToString("D", CultureInfo.InvariantCulture));
        }

        public static TwoBString FromValue(long val)
        {
            if (val >= 0 && val < Numbers.Length)
            {
                return Numbers[val];
            }

            return FromValue(val.ToString("D", CultureInfo.InvariantCulture));
        }

        public static TwoBString FromValue(int val)
        {
            if (val >= 0 && val < Numbers.Length)
            {
                return Numbers[val];
            }

            return FromValue(val.ToString("D", CultureInfo.InvariantCulture));
        }

        public static TwoBString FromValue(bool val)
        {
            return val ? True : False;
        }

        public static TwoBString FromValue(string val, bool disableCaching = false)
        {
            if (string.IsNullOrEmpty(val))
            {
                return Empty;
            }

            var instance = new TwoBString(val);
            var cache = s_cache;
            if (!disableCaching && cache != null)
            {
                int strLen = val.Length;
                if (strLen > MinimumCacheSize)
                {
                    var cachedInstance = cache.GetOrCreate(instance, entry =>
                    {
                        entry.Size = strLen;
                        return instance;
                    });
                    return cachedInstance;
                }
            }

            return instance;
        }

        public static void SetCache(IMemoryCache cache)
        {
            s_cache = cache;
        }
    }
}
