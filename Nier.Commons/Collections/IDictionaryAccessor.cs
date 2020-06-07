using System.Collections.Generic;

namespace Nier.Commons.Collections
{
    /// <summary>
    /// Adaptor interface to hide difference between <see cref="IDictionary{TKey,TValue}"/>
    /// and <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    internal interface IDictionaryAccessor<TKey, TVal> : IEnumerable<KeyValuePair<TKey, TVal>>
    {
        int Count { get; }
        bool TryGetValue(TKey key, out TVal val);

        IDictionary<TKey, TVal> ToDictionary();
    }
}
