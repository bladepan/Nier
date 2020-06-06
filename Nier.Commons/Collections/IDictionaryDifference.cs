using System.Collections.Generic;

namespace Nier.Commons.Collections
{
    /// <summary>
    /// Describe the difference between 2 dictionaries.
    /// </summary>
    /// <typeparam name="TKey">Type of dictionary keys</typeparam>
    /// <typeparam name="TVal">Type of dictionary values</typeparam>
    public interface IDictionaryDifference<TKey, TVal>
    {
        /// <summary>
        /// Entries that only appear on the left dictionary
        /// </summary>
        IDictionary<TKey, TVal> EntriesOnlyOnLeft { get; }

        /// <summary>
        /// Entries that only appear on the right dictionary
        /// </summary>
        IDictionary<TKey, TVal> EntriesOnlyOnRight { get; }

        /// <summary>
        /// Entries with keys that appear on both dictionaries with equal values
        /// </summary>
        IDictionary<TKey, TVal> EntriesInCommon { get; }

        /// <summary>
        /// Entries with keys that appear on both dictionaries with different values
        /// </summary>
        IDictionary<TKey, IDictionaryValueDifference<TVal>> EntriesDiffering { get; }
    }

    /// <summary>
    /// Describe the difference with 2 dictionary values
    /// </summary>
    /// <typeparam name="TVal">Type of value</typeparam>
    public interface IDictionaryValueDifference<TVal>
    {
        TVal LeftValue { get; }
        TVal RightValue { get; }
    }
}
