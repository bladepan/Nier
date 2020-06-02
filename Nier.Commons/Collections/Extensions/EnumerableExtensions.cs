using System.Collections.Generic;
using System.Text;

namespace Nier.Commons.Collections.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Readable string representation of a IEnumerable object with all the values.
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
    }
}
