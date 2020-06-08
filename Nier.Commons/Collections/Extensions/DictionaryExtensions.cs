using System.Collections.Generic;
using System.Text;
using Nier.Commons.Extensions;

namespace Nier.Commons.Collections.Extensions
{
    /// <summary>
    /// Utility methods for <see cref="IDictionary{TKey,TValue}"/> and <see cref="IReadOnlyDictionary{TKey,TValue}"/> type.
    ///
    /// Methods deal with IReadOnlyDictionary have "ReadOnly" prefix or suffix. This is because types like <see cref="Dictionary{TKey,TValue}"/>
    /// implements both interfaces, it will create ambiguous reference if the same name is used.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// <see cref="dict1"/> has the same key value pairs as <see cref="dict2"/>, or they
        /// are both null/empty.
        /// </summary>
        /// <param name="dict1">can be null</param>
        /// <param name="dict2">can be null</param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static bool IsEquivalentTo<TKey, TValue>(this IDictionary<TKey, TValue> dict1,
            IDictionary<TKey, TValue> dict2)
        {
            return IsEquivalentTo(new DictionaryAccessor<TKey, TValue>(dict1),
                new DictionaryAccessor<TKey, TValue>(dict2));
        }

        /// <summary>
        /// same as <see cref="IsEquivalentTo{TKey,TValue}"/>, but for IReadOnlyDictionary types.
        /// </summary>
        /// <param name="dict1"></param>
        /// <param name="dict2"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static bool ReadOnlyIsEquivalentTo<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict1,
            IReadOnlyDictionary<TKey, TValue> dict2)
        {
            return IsEquivalentTo(new ReadOnlyDictionaryAccessor<TKey, TValue>(dict1),
                new ReadOnlyDictionaryAccessor<TKey, TValue>(dict2));
        }

        /// <summary>
        /// Same as <see cref="IsEquivalentTo{TKey,TValue}"/>, but compares IDictionary with IReadOnlyDictionary.
        /// </summary>
        /// <param name="dict1"></param>
        /// <param name="dict2"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static bool IsEquivalentToReadonly<TKey, TValue>(this IDictionary<TKey, TValue> dict1,
            IReadOnlyDictionary<TKey, TValue> dict2)
        {
            return IsEquivalentTo(new DictionaryAccessor<TKey, TValue>(dict1),
                new ReadOnlyDictionaryAccessor<TKey, TValue>(dict2));
        }

        private static bool IsEquivalentTo<TKey, TValue>(IDictionaryAccessor<TKey, TValue> dict1,
            IDictionaryAccessor<TKey, TValue> dict2)
        {
            int len1 = dict1.Count;
            int len2 = dict2.Count;
            if (len1 != len2)
            {
                return false;
            }

            if (len1 > 0)
            {
                // dict1 and dict2 can't be null from this point
                foreach (KeyValuePair<TKey, TValue> kv in dict1)
                {
                    if (dict2.TryGetValue(kv.Key, out TValue val2))
                    {
                        if (!Equals(kv.Value, val2))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Readable string representation contains key values of a dictionary.
        /// {{"key1", 1}, {"key2", 2}} => "Dictionary<String,Int32>{key1=1, key2=2}"
        /// </summary>
        /// <param name="dict">dictionary, can be null.</param>
        /// <typeparam name="TKey">type of key</typeparam>
        /// <typeparam name="TValue">type of value</typeparam>
        /// <returns>readable string representation of the dictionary</returns>
        public static string ToReadableString<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (dict == null)
            {
                stringBuilder.Append("IDictionary<").Append(typeof(TKey).ToReadableString()).Append(",")
                    .Append(typeof(TValue).ToReadableString())
                    .Append("> null");
            }
            else
            {
                stringBuilder.Append(dict.GetType().ToReadableString()).Append("{");
                bool firstValue = true;
                foreach (KeyValuePair<TKey, TValue> keyValuePair in dict)
                {
                    if (firstValue)
                    {
                        firstValue = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(keyValuePair.Key?.ToString() ?? "null");
                    stringBuilder.Append('=');
                    stringBuilder.Append(keyValuePair.Value?.ToString() ?? "null");
                }

                stringBuilder.Append('}');
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get difference of this dictionary (left) to another (right). See <see cref="IDictionaryDifference{TKey,TVal}"/>.
        /// </summary>
        /// <param name="left">The left dictionary. can be null.</param>
        /// <param name="right">The right dictionary. can be null.</param>
        /// <typeparam name="TKey">Type of the dictionary keys</typeparam>
        /// <typeparam name="TVal">Type of the dictionary values</typeparam>
        /// <returns>The difference between 2 dictionaries</returns>
        public static IDictionaryDifference<TKey, TVal> GetDifference<TKey, TVal>(this IDictionary<TKey, TVal> left,
            IDictionary<TKey, TVal> right)
        {
            return GetDifference(new DictionaryAccessor<TKey, TVal>(left), new DictionaryAccessor<TKey, TVal>(right));
        }

        /// <summary>
        /// Same as <see cref="GetDifference{TKey,TVal}"/>, for IReadOnlyDictionary types.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <returns></returns>
        public static IDictionaryDifference<TKey, TVal> ReadOnlyGetDifference<TKey, TVal>(
            this IReadOnlyDictionary<TKey, TVal> left,
            IReadOnlyDictionary<TKey, TVal> right)
        {
            return GetDifference(new ReadOnlyDictionaryAccessor<TKey, TVal>(left),
                new ReadOnlyDictionaryAccessor<TKey, TVal>(right));
        }

        private static IDictionaryDifference<TKey, TVal> GetDifference<TKey, TVal>(
            IDictionaryAccessor<TKey, TVal> left,
            IDictionaryAccessor<TKey, TVal> right)
        {
            IDictionary<TKey, TVal> entriesOnlyOnLeft = new Dictionary<TKey, TVal>();
            IDictionary<TKey, TVal> entriesOnlyOnRight = new Dictionary<TKey, TVal>();
            var entriesInCommon = new Dictionary<TKey, TVal>();
            var entriesDiffering = new Dictionary<TKey, IDictionaryValueDifference<TVal>>();
            int leftSize = left.Count;
            int rightSize = right.Count;
            if (leftSize != 0 && rightSize != 0)
            {
                foreach (KeyValuePair<TKey, TVal> leftKeyVal in left)
                {
                    TKey key = leftKeyVal.Key;
                    TVal leftVal = leftKeyVal.Value;
                    if (right.TryGetValue(key, out TVal rightVal))
                    {
                        if (Equals(leftVal, rightVal))
                        {
                            entriesInCommon[key] = leftVal;
                        }
                        else
                        {
                            entriesDiffering[key] = new DictionaryValueDifference<TVal>(leftVal, rightVal);
                        }
                    }
                    else
                    {
                        entriesOnlyOnLeft[key] = leftVal;
                    }
                }

                foreach (KeyValuePair<TKey, TVal> rightKeyVal in right)
                {
                    TKey key = rightKeyVal.Key;
                    if (!entriesOnlyOnLeft.ContainsKey(key) && !entriesInCommon.ContainsKey(key) &&
                        !entriesDiffering.ContainsKey(key))
                    {
                        entriesOnlyOnRight[key] = rightKeyVal.Value;
                    }
                }
            }
            else if (leftSize != 0)
            {
                entriesOnlyOnLeft = left.ToDictionary();
            }
            else if (rightSize != 0)
            {
                entriesOnlyOnRight = right.ToDictionary();
            }

            return new DictionaryDifference<TKey, TVal>(entriesOnlyOnLeft, entriesOnlyOnRight, entriesInCommon,
                entriesDiffering);
        }
    }
}
