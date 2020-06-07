using System;

namespace Nier.Commons.Collections.Extensions
{
    /// <summary>
    /// Utility methods for <see cref="IDictionaryDifference{TKey,TVal}"/> type.
    /// </summary>
    public static class DictionaryDifferenceExtensions
    {
        /// <summary>
        /// If the difference object represents any difference between 2 dictionaries.
        /// </summary>
        /// <param name="difference"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">the difference object is null</exception>
        public static bool IsEmpty<TKey, TVal>(this IDictionaryDifference<TKey, TVal> difference)
        {
            if (difference == null)
            {
                throw new ArgumentNullException(nameof(difference));
            }

            return difference.EntriesOnlyOnLeft.Count == 0 && difference.EntriesOnlyOnRight.Count == 0 &&
                   difference.EntriesDiffering.Count == 0;
        }
    }
}
