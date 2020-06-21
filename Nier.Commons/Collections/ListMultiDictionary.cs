using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nier.Commons.Collections.Extensions;
using Nier.Commons.Extensions;

namespace Nier.Commons.Collections
{
    /// <summary>
    /// A <see cref="IMultiDictionary{TKey,TValue}"/> implementation based on List.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <inheritdoc cref="IMultiDictionary{TKey,TValue}"/>
    public class ListMultiDictionary<TKey, TValue> : IMultiDictionary<TKey, TValue>,
        IEquatable<ListMultiDictionary<TKey, TValue>>
    {
        private readonly Dictionary<TKey, List<TValue>> _inner = new Dictionary<TKey, List<TValue>>();

        public IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> GetEnumerator() =>
            _inner.Select(pair =>
                new KeyValuePair<TKey, IReadOnlyCollection<TValue>>(pair.Key, pair.Value)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<TKey, IReadOnlyCollection<TValue>> item) =>
            _inner.Add(item.Key, new List<TValue>(item.Value));

        public void Clear() => _inner.Clear();

        public bool Contains(KeyValuePair<TKey, IReadOnlyCollection<TValue>> item)
        {
            IReadOnlyCollection<TValue> val = item.Value;
            if (val == null)
            {
                return false;
            }

            if (_inner.TryGetValue(item.Key, out List<TValue> list))
            {
                return list.SequenceEqual(val);
            }

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, IReadOnlyCollection<TValue>>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex, ExceptionResources.NeedNonNegNum);
            }

            int arrayLen = array.Length;
            if (arrayIndex >= arrayLen || arrayLen - arrayIndex < Count)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), ExceptionResources.ArrayPlusOffTooSmall);
            }

            int index = arrayIndex;
            foreach (KeyValuePair<TKey, List<TValue>> pair in _inner)
            {
                array[index] =
                    new KeyValuePair<TKey, IReadOnlyCollection<TValue>>(pair.Key, pair.Value);
            }
        }

        public bool Remove(KeyValuePair<TKey, IReadOnlyCollection<TValue>> item)
        {
            if (Contains(item))
            {
                return _inner.Remove(item.Key);
            }

            return false;
        }

        public int Count
        {
            get => _inner.Count;
        }

        public bool IsReadOnly { get; } = false;

        public void Add(TKey key, IReadOnlyCollection<TValue> value)
        {
            _inner.Add(key, new List<TValue>(value));
        }

        public bool ContainsKey(TKey key) => _inner.ContainsKey(key);

        public bool Remove(TKey key) => _inner.Remove(key);

        public bool TryGetValue(TKey key, out IReadOnlyCollection<TValue> value)
        {
            if (_inner.TryGetValue(key, out List<TValue> existingValue))
            {
                value = existingValue;
                return true;
            }

            value = null;
            return false;
        }

        public IReadOnlyCollection<TValue> this[TKey key]
        {
            get
            {
                var list = _inner[key];
                return list;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (value == null || value.Count == 0)
                {
                    _ = Remove(key);
                    return;
                }

                _inner[key] = new List<TValue>(value);
            }
        }

        public ICollection<TKey> Keys
        {
            get => _inner.Keys;
        }

        public ICollection<IReadOnlyCollection<TValue>> Values
        {
            get => _inner.Values.Select(val =>
            {
                IReadOnlyCollection<TValue> readOnlyCollection = val;
                return readOnlyCollection;
            }).ToList();
        }

        public bool Add(TKey key, TValue value)
        {
            return UpdateInner(key, list =>
            {
                list.Add(value);
                return 1;
            }) == 1;
        }

        public bool AddAll(TKey key, IEnumerable<TValue> values)
        {
            return UpdateInner(key, list =>
            {
                int previousCount = list.Count;
                list.AddRange(values);
                int count = list.Count;
                return count - previousCount;
            }) > 0;
        }

        private int UpdateInner(TKey key, Func<List<TValue>, int> action)
        {
            int result;
            if (_inner.TryGetValue(key, out List<TValue> list))
            {
                result = action(list);
                if (list.Count == 0)
                {
                    _inner.Remove(key);
                }
            }
            else
            {
                list = new List<TValue>();
                result = action(list);
                if (list.Count > 0)
                {
                    _inner.Add(key, list);
                }
            }

            return result;
        }

        public bool Remove(TKey key, TValue value)
        {
            return UpdateInner(key, list => list.Remove(value) ? 1 : 0) == 1;
        }

        public int RemoveAll(TKey key, IEnumerable<TValue> values) => UpdateInner(key, list =>
        {
            int count = 0;
            foreach (var value in values)
            {
                bool removed = list.Remove(value);
                if (removed)
                {
                    count++;
                }

                if (list.Count == 0)
                {
                    break;
                }
            }

            return count;
        });

        public IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValueEnumerator() =>
            _inner.SelectMany(kv => kv.Value,
                (pair, value) =>
                    new KeyValuePair<TKey, TValue>(pair.Key, value));

        public bool Equals(ListMultiDictionary<TKey, TValue> other) =>
            other != null && _inner.IsEquivalentTo(other._inner, (list1, list2) => list1.SequenceEqual(list2));

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is ListMultiDictionary<TKey, TValue> another)
            {
                return Equals(another);
            }

            return false;
        }

        public override int GetHashCode() => _inner.GetHashCode();

        public override string ToString() => _inner.ToReadableString(GetType().ToReadableString(),
            key => key.ToString(),
            list => list?.ToReadableString(string.Empty) ?? "null");
    }
}
