using System;
using System.Collections.Generic;
using System.Text;

namespace Nier.Collections.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool IsEquivalentTo<TKey, TValue>(this IDictionary<TKey, TValue> dict1,
            IDictionary<TKey, TValue> dict2)
        {
            int len1 = dict1?.Count ?? 0;
            int len2 = dict2?.Count ?? 0;
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
                stringBuilder.Append("IDictionary<").Append(typeof(TKey).Name).Append(",").Append(typeof(TValue).Name)
                    .Append("> null");
            }
            else
            {
                // something like Dictionary`2
                string dictTypeName = dict.GetType().Name;
                int backTickIndex = dictTypeName.IndexOf("`", StringComparison.Ordinal);
                if (backTickIndex >= 0)
                {
                    dictTypeName = dictTypeName.Remove(backTickIndex);
                }

                stringBuilder.Append(dictTypeName).Append("<")
                    .Append(typeof(TKey).Name).Append(",").Append(typeof(TValue).Name)
                    .Append(">{");
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
    }
}
