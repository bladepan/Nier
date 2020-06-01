using System.Collections.Generic;

namespace Nier.Collections.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool AreEquivalent<TKey, TValue>(this IDictionary<TKey, TValue> dict1,
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
    }
}
