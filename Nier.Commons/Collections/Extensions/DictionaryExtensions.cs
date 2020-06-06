using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Nier.Commons.Collections.Extensions
{
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

        /// <summary>
        /// Get difference of this dictionary (left) to another (right).
        /// </summary>
        /// <param name="left">The left dictionary. can be null.</param>
        /// <param name="right">The right dictionary. can be null.</param>
        /// <typeparam name="TKey">Type of the dictionary keys</typeparam>
        /// <typeparam name="TVal">Type of the dictionary values</typeparam>
        /// <returns>The difference between 2 dictionaries</returns>
        public static IDictionaryDifference<TKey, TVal> GetDifference<TKey, TVal>(this IDictionary<TKey, TVal> left,
            IDictionary<TKey, TVal> right)
        {
            IDictionary<TKey, TVal> entriesOnlyOnLeft = new Dictionary<TKey, TVal>();
            IDictionary<TKey, TVal> entriesOnlyOnRight = new Dictionary<TKey, TVal>();
            var entriesInCommon = new Dictionary<TKey, TVal>();
            var entriesDiffering = new Dictionary<TKey, IDictionaryValueDifference<TVal>>();
            int leftSize = left?.Count ?? 0;
            int rightSize = right?.Count ?? 0;
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
                entriesOnlyOnLeft = left;
            }
            else if (rightSize != 0)
            {
                entriesOnlyOnRight = right;
            }

            return new DictionaryDifference<TKey, TVal>(entriesOnlyOnLeft, entriesOnlyOnRight, entriesInCommon,
                entriesDiffering);
        }

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

    internal class DictionaryDifference<TKey, TVal> : IDictionaryDifference<TKey, TVal>
    {
        public IDictionary<TKey, TVal> EntriesOnlyOnLeft { get; }
        public IDictionary<TKey, TVal> EntriesOnlyOnRight { get; }
        public IDictionary<TKey, TVal> EntriesInCommon { get; }
        public IDictionary<TKey, IDictionaryValueDifference<TVal>> EntriesDiffering { get; }

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
            EntriesOnlyOnLeft.IsEquivalentTo(other.EntriesOnlyOnLeft) &&
            EntriesOnlyOnRight.IsEquivalentTo(other.EntriesOnlyOnRight) &&
            EntriesInCommon.IsEquivalentTo(other.EntriesInCommon) &&
            EntriesDiffering.IsEquivalentTo(other.EntriesDiffering);

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

    internal class DictionaryValueDifference<TVal> : IDictionaryValueDifference<TVal>
    {
        public TVal LeftValue { get; }
        public TVal RightValue { get; }

        public DictionaryValueDifference(TVal leftValue, TVal rightValue)
        {
            LeftValue = leftValue;
            RightValue = rightValue;
        }

        private bool Equals(IDictionaryValueDifference<TVal> other) =>
            Equals(LeftValue, other.LeftValue) && Equals(RightValue, other.RightValue);

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

            if (obj is IDictionaryValueDifference<TVal> another)
            {
                return Equals(another);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TVal>.Default.GetHashCode(LeftValue) * 397) ^
                       EqualityComparer<TVal>.Default.GetHashCode(RightValue);
            }
        }
    }
}
