using System;
using System.Collections;
using System.Collections.Generic;
using Nier.Commons.Collections.Extensions;

namespace Nier.Commons.Collections
{
    /// <summary>
    /// Implementation of <see cref="IBiDirectionDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class BiDirectionDictionary<TKey, TValue> : IBiDirectionDictionary<TKey, TValue>
    {
        private readonly BiDirectionDictionaryView<TKey, TValue> _view;
        public IBiDirectionDictionary<TValue, TKey> ReverseDirection { get; }

        private BiDirectionDictionary(BiDirectionDictionaryView<TKey, TValue> view,
            IBiDirectionDictionary<TValue, TKey> reverseView)
        {
            _view = view;
            ReverseDirection = reverseView;
        }

        public BiDirectionDictionary()
        {
            var dict = new Dictionary<TKey, TValue>();
            var reverseDict = new Dictionary<TValue, TKey>();
            _view = new BiDirectionDictionaryView<TKey, TValue>(dict, reverseDict);
            ReverseDirection =
                new BiDirectionDictionary<TValue, TKey>(new BiDirectionDictionaryView<TValue, TKey>(reverseDict, dict),
                    this);
        }

        /// <summary>
        /// Create a instance based on source. Throw ArgumentException when there is duplicated
        /// key or value in source.
        /// </summary>
        /// <param name="source"></param>
        public BiDirectionDictionary(IEnumerable<KeyValuePair<TKey, TValue>> source) : this()
        {
            if (source != null)
            {
                foreach (KeyValuePair<TKey, TValue> keyValuePair in source)
                {
                    Add(keyValuePair);
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _view.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _view.GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item) => _view.Add(item);

        public void Clear() => _view.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) => _view.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _view.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) => _view.Remove(item);

        public int Count
        {
            get => _view.Count;
        }

        public bool IsReadOnly
        {
            get => _view.IsReadOnly;
        }

        public void Add(TKey key, TValue value) => _view.Add(key, value);

        public bool ContainsKey(TKey key) => _view.ContainsKey(key);

        public bool Remove(TKey key) => _view.Remove(key);

        public bool TryGetValue(TKey key, out TValue value) => _view.TryGetValue(key, out value);

        public TValue this[TKey key]
        {
            get => _view[key];
            set => _view[key] = value;
        }

        public ICollection<TKey> Keys
        {
            get => _view.Keys;
        }

        public ICollection<TValue> Values
        {
            get => _view.Values;
        }

        public override string ToString() => this.ToReadableString();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is BiDirectionDictionary<TKey, TValue> other)
            {
                return this.IsEquivalentTo(other);
            }

            return false;
        }

        public override int GetHashCode() => _view.GetHashCode();
    }

    internal class BiDirectionDictionaryView<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;
        private readonly IDictionary<TValue, TKey> _reverseDictionary;

        public BiDirectionDictionaryView(IDictionary<TKey, TValue> dictionary,
            IDictionary<TValue, TKey> reverseDictionary)
        {
            _dictionary = dictionary;
            _reverseDictionary = reverseDictionary;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dictionary.Clear();
            _reverseDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            _dictionary.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool result = _dictionary.Remove(item);
            if (result)
            {
                bool reverseResult = _reverseDictionary.Remove(item.Value);
                if (!reverseResult)
                {
                    throw new InvalidOperationException(
                        "Illegal state in BidirectionalDictionary. Item removed from one direction but failed in the reverse direction.");
                }
            }

            return result;
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly { get; } = false;

        public void Add(TKey key, TValue value)
        {
            if (_dictionary.ContainsKey(key))
            {
                throw new ArgumentException("Duplicate key.");
            }

            if (_reverseDictionary.ContainsKey(value))
            {
                throw new ArgumentException("Duplicate value.");
            }

            _dictionary.Add(key, value);
            _reverseDictionary.Add(value, key);
        }

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public bool Remove(TKey key)
        {
            if (_dictionary.TryGetValue(key, out TValue value))
            {
                bool result = _dictionary.Remove(key);
                if (result)
                {
                    bool reverseResult = _reverseDictionary.Remove(value);
                    if (!reverseResult)
                    {
                        throw new InvalidOperationException(
                            "Illegal state in BidirectionalDictionary. Item removed from one direction but failed in the reverse direction.");
                    }

                    return true;
                }
                else
                {
                    throw new InvalidOperationException(
                        "Illegal state in BidirectionalDictionary. Concurrent modification happened during Remove.");
                }
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                if (_dictionary.TryGetValue(key, out TValue existingValue))
                {
                    _dictionary.Remove(key);
                    _reverseDictionary.Remove(existingValue);
                }

                if (_reverseDictionary.TryGetValue(value, out TKey existingKey))
                {
                    _dictionary.Remove(existingKey);
                    _reverseDictionary.Remove(value);
                }

                _dictionary.Add(key, value);
                _reverseDictionary.Add(value, key);
            }
        }

        public ICollection<TKey> Keys
        {
            get { return _dictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return _dictionary.Values; }
        }

        public override int GetHashCode() => _dictionary.GetHashCode();
    }
}
