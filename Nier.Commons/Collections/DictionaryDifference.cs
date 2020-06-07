using System.Collections.Generic;
using System.Collections.ObjectModel;
using Nier.Commons.Collections.Extensions;

namespace Nier.Commons.Collections
{
    internal class DictionaryDifference<TKey, TVal> : IDictionaryDifference<TKey, TVal>
    {
        public IReadOnlyDictionary<TKey, TVal> EntriesOnlyOnLeft { get; }
        public IReadOnlyDictionary<TKey, TVal> EntriesOnlyOnRight { get; }
        public IReadOnlyDictionary<TKey, TVal> EntriesInCommon { get; }
        public IReadOnlyDictionary<TKey, IDictionaryValueDifference<TVal>> EntriesDiffering { get; }

        public DictionaryDifference(IDictionary<TKey, TVal> entriesOnlyOnLeft,
            IDictionary<TKey, TVal> entriesOnlyOnRight, IDictionary<TKey, TVal> entriesInCommon,
            IDictionary<TKey, IDictionaryValueDifference<TVal>> entriesDiffering)
        {
            EntriesOnlyOnLeft = new ReadOnlyDictionary<TKey, TVal>(entriesOnlyOnLeft);
            EntriesOnlyOnRight = new ReadOnlyDictionary<TKey, TVal>(entriesOnlyOnRight);
            EntriesInCommon = new ReadOnlyDictionary<TKey, TVal>(entriesInCommon);
            EntriesDiffering = new ReadOnlyDictionary<TKey, IDictionaryValueDifference<TVal>>(entriesDiffering);
        }

        private bool Equals(IDictionaryDifference<TKey, TVal> other) =>
            EntriesOnlyOnLeft.ReadOnlyIsEquivalentTo(other.EntriesOnlyOnLeft) &&
            EntriesOnlyOnRight.ReadOnlyIsEquivalentTo(other.EntriesOnlyOnRight) &&
            EntriesInCommon.ReadOnlyIsEquivalentTo(other.EntriesInCommon) &&
            EntriesDiffering.ReadOnlyIsEquivalentTo(other.EntriesDiffering);

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

            if (obj is IDictionaryDifference<TKey, TVal> other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = EntriesOnlyOnLeft.GetHashCode();
                hashCode = (hashCode * 397) ^ EntriesOnlyOnRight.GetHashCode();
                hashCode = (hashCode * 397) ^ EntriesInCommon.GetHashCode();
                hashCode = (hashCode * 397) ^ EntriesDiffering.GetHashCode();
                return hashCode;
            }
        }
    }
}
