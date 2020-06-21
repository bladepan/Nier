using System.Collections.Generic;

namespace Nier.Commons.Collections
{
    /// <summary>
    /// Similar to <seealso cref="IDictionary{TKey,TValue}"/> but each key can be associated with multiple values.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IMultiDictionary<TKey, TValue> : IDictionary<TKey, IReadOnlyCollection<TValue>>
    {
        /// <summary>
        /// Add a single value to Key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Add(TKey key, TValue value);

        /// <summary>
        /// Add multiple values to key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        bool AddAll(TKey key, IEnumerable<TValue> values);

        /// <summary>
        /// Remove value from Key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>true if the value is removed.</returns>
        bool Remove(TKey key, TValue value);

        /// <summary>
        /// Remove all elements in values associated to key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns>The number of values removed.</returns>
        int RemoveAll(TKey key, IEnumerable<TValue> values);

        /// <summary>
        /// A enumerator with key value pairs.
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValueEnumerator();
    }
}
