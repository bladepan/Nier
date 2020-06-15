using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nier.Commons.Collections.Extensions;
using Nier.Commons.Extensions;

namespace Nier.Commons.Collections
{
    /// <summary>
    /// A implementation of <see cref="IMultiSet{TValue}"/>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <inheritdoc cref="IMultiSet{TValue}"/>
    public class MultiSet<TValue> : IMultiSet<TValue>
    {
        private readonly Dictionary<NullableKey<TValue>, int> _values;

        public MultiSet()
        {
            _values = new Dictionary<NullableKey<TValue>, int>();
        }

        public IEnumerator<TValue> GetEnumerator() =>
            new MultiSetEnumerator<TValue>(_values.GetEnumerator());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(TValue item)
        {
            _ = SetItemCount(item, itemCount => itemCount + 1);
        }

        /// <summary>
        /// set the item count
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newCountFunc">current item count to new count value</param>
        /// <returns></returns>
        private int SetItemCount(TValue item, Func<int, int> newCountFunc)
        {
            int newItemCount;
            int oldItemCount;

            NullableKey<TValue> key = new NullableKey<TValue>(item);
            if (_values.TryGetValue(key, out int currentItemCount))
            {
                oldItemCount = currentItemCount;
                newItemCount = newCountFunc(currentItemCount);
            }
            else
            {
                oldItemCount = 0;
                newItemCount = newCountFunc(0);
            }

            if (oldItemCount != newItemCount)
            {
                if (newItemCount == 0)
                {
                    _values.Remove(key);
                }
                else
                {
                    _values[key] = newItemCount;
                }

                Count = Count - oldItemCount + newItemCount;
            }

            return oldItemCount;
        }

        public int Add(TValue item, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Non-negative number required.");
            }

            return SetItemCount(item, itemCount => itemCount + count);
        }

        public int GetItemCount(TValue item)
        {
            if (_values.TryGetValue(new NullableKey<TValue>(item), out int count))
            {
                return count;
            }

            return 0;
        }

        public int SetItemCount(TValue item, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Non-negative number required.");
            }

            return SetItemCount(item, _ => count);
        }

        public bool SetItemCount(TValue item, int expectedCount, int newCount)
        {
            if (newCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newCount), newCount, "Non-negative number required.");
            }

            if (expectedCount < 0 || expectedCount == newCount)
            {
                return false;
            }

            int oldCount = SetItemCount(item, itemCount => itemCount == expectedCount ? newCount : itemCount);

            return oldCount == expectedCount;
        }

        public int Remove(TValue item, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Non-negative number required.");
            }

            return SetItemCount(item, itemCount =>
            {
                int itemLeft = itemCount - count;
                return itemLeft > 0 ? itemLeft : 0;
            });
        }

        public IEnumerable<TValue> ItemSet
        {
            get { return _values.Select(kv => kv.Key.Key); }
        }

        public IEnumerable<KeyValuePair<TValue, int>> EntrySet
        {
            get { return _values.Select(kv => new KeyValuePair<TValue, int>(kv.Key.Key, kv.Value)); }
        }

        public void Clear()
        {
            Count = 0;
            _values.Clear();
        }

        public bool Contains(TValue item) => _values.ContainsKey(new NullableKey<TValue>(item));

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex, "Non-negative number required.");
            }

            int arrayLen = array.Length;
            if (arrayIndex >= arrayLen || arrayLen - arrayIndex < Count)
            {
                throw new ArgumentException(
                    "Destination array is not long enough to copy all the items in the collection. Check array index and length.");
            }

            int index = arrayIndex;
            foreach (TValue value in this)
            {
                array[index] = value;
                index++;
            }
        }

        public bool Remove(TValue item)
        {
            return Remove(item, 1) != 0;
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get; } = false;

        public override string ToString() =>
            this.ToStringBuilder().Add("Entries", _values.ToReadableString("")).ToString();

        private bool Equals(MultiSet<TValue> other) => _values.IsEquivalentTo(other._values);

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

            if (obj is MultiSet<TValue> another)
            {
                return Equals(another);
            }

            return false;
        }

        public override int GetHashCode() =>  _values.GetHashCode();
    }

    /// <summary>
    /// Dictionary does not allow null as key.
    /// Wrap key in this to get around it.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    internal readonly struct NullableKey<TKey> : IEquatable<NullableKey<TKey>>
    {
        public TKey Key { get; }

        public NullableKey(TKey key)
        {
            Key = key;
        }

        public bool Equals(NullableKey<TKey> other) => Equals(Key, other.Key);

        public override bool Equals(object obj) => obj is NullableKey<TKey> other && Equals(other);

        public override int GetHashCode() => Key == null ? 0 : Key.GetHashCode();

        public override string ToString() => Key == null ? "null" : Key.ToString();
    }

    internal class MultiSetEnumerator<TValue> : IEnumerator<TValue>
    {
        private readonly IEnumerator<KeyValuePair<NullableKey<TValue>, int>> _inner;
        private int _index;
        private int _count;

        public MultiSetEnumerator(IEnumerator<KeyValuePair<NullableKey<TValue>, int>> inner)
        {
            _inner = inner;
        }

        public bool MoveNext()
        {
            if (_index == _count)
            {
                bool innerResult = _inner.MoveNext();
                if (!innerResult)
                {
                    return false;
                }

                KeyValuePair<NullableKey<TValue>, int> innerCurrent = _inner.Current;
                Current = innerCurrent.Key.Key;
                _count = innerCurrent.Value;
                _index = 1;
                return true;
            }

            _index++;
            return true;
        }

        public void Reset()
        {
            _count = 0;
            _index = 0;
            _inner.Reset();
        }

        public TValue Current { get; private set; }
        object IEnumerator.Current => Current;
        public void Dispose() => _inner.Dispose();
    }
}
