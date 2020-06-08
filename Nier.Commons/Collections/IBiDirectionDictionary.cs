using System.Collections.Generic;

namespace Nier.Commons.Collections
{
    /// <summary>
    /// Bi-direction dictionary provides O(1) cost look up and set key value paris in
    /// both direction.
    /// Both keys and values are guaranteed to be unique.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IBiDirectionDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Return a instance so the caller can manipulate the dictionary in reverse direction.
        /// </summary>
        IBiDirectionDictionary<TValue, TKey> ReverseDirection { get; }
    }
}
