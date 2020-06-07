using System;
using System.Collections;
using System.Collections.Generic;

namespace Nier.Commons.Collections
{
    internal class DictionaryAccessor<TKey, TVal> : IDictionaryAccessor<TKey, TVal>
    {
        private readonly IDictionary<TKey, TVal> _dictionary;
        private readonly IEnumerable<KeyValuePair<TKey, TVal>> _enumerator;

        public DictionaryAccessor(IDictionary<TKey, TVal> dictionary)
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

        public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator() => _enumerator.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _enumerator.GetEnumerator();

        public int Count => _dictionary?.Count ?? 0;

        public bool TryGetValue(TKey key, out TVal val)
        {
            val = default;
            return _dictionary != null && _dictionary.TryGetValue(key, out val);
        }

        public IDictionary<TKey, TVal> ToDictionary() => _dictionary ?? new Dictionary<TKey, TVal>();
    }
}
