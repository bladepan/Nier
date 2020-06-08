using System;
using System.Text;

namespace Nier.Commons.Extensions
{
    /// <summary>
    /// Utility methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Readable string representation of a type.
        /// For example,
        /// (new Dictionary<DateTimeOffset, IReadOnlyDictionary<string, int>>[3, 3]).GetType().ToReadableString()
        /// => "Dictionary<DateTimeOffset,IReadOnlyDictionary<String,Int32>>[,]",
        /// </summary>
        /// <param name="type">the type object. can not be null.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">type is null</exception>
        public static string ToReadableString(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsArray)
            {
                Type eleType = type.GetElementType();
                int arrayDimensions = type.GetArrayRank();
                string result = ToReadableString(eleType) + "[";
                for (int i = 0; i < arrayDimensions - 1; i++)
                {
                    result += ",";
                }

                result += "]";
                return result;
            }

            string typeName = type.Name;
            if (type.IsGenericType)
            {
                // something like Dictionary`2
                int backTickIndex = typeName.IndexOf("`", StringComparison.Ordinal);
                string simpleTypeName = typeName;
                if (backTickIndex >= 0)
                {
                    simpleTypeName = typeName.Remove(backTickIndex);
                }

                var genericArgs = type.GetGenericArguments();
                StringBuilder stringBuilder = new StringBuilder(simpleTypeName);
                stringBuilder.Append("<");
                bool firstArg = true;
                foreach (Type genericArg in genericArgs)
                {
                    if (!firstArg)
                    {
                        stringBuilder.Append(",");
                    }
                    else
                    {
                        firstArg = false;
                    }

                    stringBuilder.Append(ToReadableString(genericArg));
                }

                stringBuilder.Append(">");
                return stringBuilder.ToString();
            }

            return typeName;
        }
    }
}
