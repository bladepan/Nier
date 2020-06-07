using System;
using System.Collections.Generic;
using System.Text;

namespace Nier.Commons.Collections.Extensions
{
    /// <summary>
    /// Utility methods for <see cref="IEnumerable{T}"/> type.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Readable string representation of a IEnumerable object with all the values.
        ///
        /// new[] {"1", "2"}; => "String[]<String>[1, 2]"
        /// </summary>
        /// <param name="enumerable"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Readable string</returns>
        public static string ToReadableString<T>(this IEnumerable<T> enumerable)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (enumerable == null)
            {
                stringBuilder.Append("IEnumerable<").Append(typeof(T).Name).Append("> null");
            }
            else
            {
                string enumerableTypeName = enumerable.GetType().Name;
                stringBuilder.Append(enumerableTypeName).Append('<').Append(typeof(T).Name).Append(">[");
                bool firstValue = true;
                foreach (T value in enumerable)
                {
                    if (firstValue)
                    {
                        firstValue = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(value?.ToString() ?? "null");
                }

                stringBuilder.Append(']');
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// A variation of <see cref="SafeToDictionary{TSource, TKey, TElement}"/> that
        /// returns the elements from the source as dictionary values.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public static IDictionary<TKey, TSource> SafeToDictionary<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer = null)
        {
            return SafeToDictionary(source, keySelector, val => val, comparer);
        }

        /// <summary>
        /// Same as Linq's ToDictionary except it does not throw exception
        /// when keySelector generates duplicate keys. The new key value pair generated
        /// overrides the old one.
        /// </summary>
        /// <param name="source">the data source, can be null</param>
        /// <param name="keySelector"></param>
        /// <param name="elementSelector"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">keySelector or elementSelector is null</exception>
        public static IDictionary<TKey, TElement> SafeToDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey> comparer = null)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (elementSelector == null)
            {
                throw new ArgumentNullException(nameof(elementSelector));
            }

            var result = new Dictionary<TKey, TElement>(comparer);
            if (source != null)
            {
                foreach (TSource element in source)
                {
                    result[keySelector(element)] = elementSelector(element);
                }
            }

            return result;
        }
    }
}
