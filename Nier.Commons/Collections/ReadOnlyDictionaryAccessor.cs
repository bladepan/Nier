using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nier.Commons.Collections
{
    internal class ReadOnlyDictionaryAccessor<TKey, TVal> : IDictionaryAccessor<TKey, TVal>
    {
        private readonly IReadOnlyDictionary<TKey, TVal> _dictionary;
        private readonly IEnumerable<KeyValuePair<TKey, TVal>> _enumerator;

        public ReadOnlyDictionaryAccessor(IReadOnlyDictionary<TKey, TVal> dictionary)
        {
            _dictionary = dictionary;
            if (dictionary == null)
            {
                _enumerator = Array.Empty<KeyValuePair<TKey, TVal>>();
            }
            else
            {
                _enumerator = dictionary;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator()
        {
            return _enumerator.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => _enumerator.GetEnumerator();

        public int Count => _dictionary?.Count ?? 0;

        public bool TryGetValue(TKey key, out TVal val)
        {
            val = default;
            return _dictionary != null && _dictionary.TryGetValue(key, out val);
        }

        public IDictionary<TKey, TVal> ToDictionary()
        {
            if (_dictionary == null)
            {
                return new Dictionary<TKey, TVal>();
            }

            if (_dictionary is IDictionary<TKey, TVal> dictionary)
            {
                return dictionary;
            }

            return _dictionary.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}
