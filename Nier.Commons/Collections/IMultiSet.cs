using System.Collections.Generic;

namespace Nier.Commons.Collections
{
    /// <summary>
    /// A collection that supports order-independent equality, constant time look up.
    /// It is like <see cref="ISet{T}"/>, but may have duplicate elements.
    /// Similar to MultiSet in Guava or Bag in Apache Commons.
    ///
    /// It is useful to keep occurrences of items, i.e. counters. It provides constant time
    /// of increase, decrease, lookup and update counts of items.
    ///
    /// It uses object.Equals(obj1, obj2) to determine if two objects are the same, unless specified
    /// otherwise by implementation.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
#pragma warning disable CA1710 // Identifiers should have correct suffix. Intentional naming
    public interface IMultiSet<TValue> : ICollection<TValue>
#pragma warning restore CA1710
    {
        /// <summary>
        /// Add count number of item to the set. Return the previous count of item.
        /// </summary>
        /// <param name="item">item to be added. can be null</param>
        /// <param name="count"></param>
        /// <returns>previous count if item is already in the set, 0 otherwise</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count">count</paramref> is less than 0.</exception>
        int Add(TValue item, int count);

        /// <summary>
        /// Get number of item in the set, 0 if the item is not in the set
        /// </summary>
        /// <param name="item">item to be counted. can be null</param>
        /// <returns></returns>
        int GetItemCount(TValue item);

        /// <summary>
        /// Set count of item in set to count. Throws exception if the count is negative.
        /// </summary>
        /// <param name="item">item that the count to be updated. can be null</param>
        /// <param name="count">the new count</param>
        /// <returns>the previous count</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count">count</paramref> is less than 0.</exception>
        int SetItemCount(TValue item, int count);

        /// <summary>
        /// Set count of item to newCount if the count of item is expectedCount. Returns true
        /// if set is modified, otherwise false.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="expectedCount"></param>
        /// <param name="newCount"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="expectedCount">expectedCount</paramref> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="newCount">newCount</paramref> is less than 0.</exception>
        bool SetItemCount(TValue item, int expectedCount, int newCount);

        /// <summary>
        /// Remove count number of items from set. If count is greater than number of item in the set, all occurrences of item are removed.
        /// Return the previous count of item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count">count</paramref> is less than 0.</exception>
        int Remove(TValue item, int count);

        /// <summary>
        /// returns the unique items from the set.
        /// </summary>
        IEnumerable<TValue> ItemSet { get; }

        /// <summary>
        /// returns the key value pair of item and item count, each unique item appear only once.
        /// </summary>
        IEnumerable<KeyValuePair<TValue, int>> EntrySet { get; }
    }
}
