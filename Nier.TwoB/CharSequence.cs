using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Nier.TwoB
{
    /// <summary>
    /// A <seealso cref="string"/> wrapper with a pool that caches previous allocated instances so strings with the same
    /// content can share the same instance. It also caches hashcode to improve hashcode and equals perf.
    /// </summary>
    public class CharSequence
    {
        public static readonly CharSequence Empty = new CharSequence(string.Empty);
        public static readonly CharSequence True = new CharSequence(bool.TrueString);
        public static readonly CharSequence False = new CharSequence(bool.FalseString);

        private static IMemoryCache s_cache = new MemoryCache(new MemoryCacheOptions {SizeLimit = 64 * 1024 * 1024});

        private static readonly CharSequence[] Numbers =
            Enumerable.Range(0, 128).Select(i => new CharSequence(i.ToString("D", CultureInfo.InvariantCulture)))
                .ToArray();

        public string Value { get; }
        private readonly int _hashCode;

        private CharSequence(string value)
        {
            Value = value;
#if NETSTANDARD2_1
            _hashCode = value.GetHashCode(StringComparison.Ordinal);
#else
            _hashCode = value.GetHashCode();
#endif
        }

        public override int GetHashCode() => _hashCode;

        public override bool Equals(object obj)
        {
            if (obj is CharSequence another)
            {
                if (obj == this)
                {
                    return true;
                }

                if (another._hashCode != _hashCode)
                {
                    return false;
                }

                return Value == another.Value;
            }

            return false;
        }

        public override string ToString() => Value;

        public static CharSequence FromValue(byte val)
        {
            if (val < Numbers.Length)
            {
                return Numbers[val];
            }

            return FromValue(val.ToString("D", CultureInfo.InvariantCulture));
        }

        public static CharSequence FromValue(long val)
        {
            if (val >= 0 && val < Numbers.Length)
            {
                return Numbers[val];
            }

            return FromValue(val.ToString("D", CultureInfo.InvariantCulture));
        }

        public static CharSequence FromValue(int val)
        {
            if (val >= 0 && val < Numbers.Length)
            {
                return Numbers[val];
            }

            return FromValue(val.ToString("D", CultureInfo.InvariantCulture));
        }

        public static CharSequence FromValue(bool val)
        {
            return val ? True : False;
        }

        /// <summary>
        /// Get a instance of CharSequence with the specified value. It will try to return a cached instance first if
        /// caching is enabled.
        /// </summary>
        /// <param name="val">string value</param>
        /// <param name="disableCaching">if set true, it will always allocate a new instance instead of first trying to
        /// find a cached instance.</param>
        /// <returns></returns>
        public static CharSequence FromValue(string val, bool disableCaching = false)
        {
            if (string.IsNullOrEmpty(val))
            {
                return Empty;
            }

            var instance = new CharSequence(val);
            var cache = s_cache;
            if (!disableCaching && cache != null)
            {
                var cachedInstance = cache.GetOrCreate(instance, entry =>
                {
                    entry.Size = val.Length;
                    return instance;
                });
                return cachedInstance;
            }

            return instance;
        }

        /// <summary>
        /// set the cache that used to store CharSequence instances.
        /// </summary>
        /// <param name="cache"></param>
        public static void SetCache(IMemoryCache cache)
        {
            s_cache = cache;
        }
    }
}
