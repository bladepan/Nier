using System;
using System.Collections.Generic;
using System.Text;

namespace Nier.Commons.Extensions
{
    /// <summary>
    /// Utility methods for <see cref="object"/> type.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Null safe ToString.
        /// </summary>
        /// <param name="obj">may be null</param>
        /// <returns></returns>
        public static string SafeToString(this object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            return obj.ToString();
        }

        /// <summary>
        /// Create a ToStringBuilder instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">obj is null</exception>
        public static ToStringBuilder ToStringBuilder(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            return new ToStringBuilder(obj.GetType().ToReadableString());
        }

        /// <summary>
        /// Create a ToStringBuilder instance.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ToStringBuilder ToStringBuilder(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                throw new ArgumentNullException(nameof(className));
            }
            return new ToStringBuilder(className);
        }
    }

    /// <summary>
    /// NotThreadSafe. A helper class to implement ToString methods.
    /// </summary>
    public class ToStringBuilder
    {
        private readonly string _className;
        private bool _omitNullValues;
        private LinkedList<KeyValuePair<string, object>> _values;

        internal ToStringBuilder(string className)
        {
            _className = className;
        }

        /// <summary>
        /// Skip null values in ToString.
        /// </summary>
        /// <returns>The ToStringBuilder itself.</returns>
        public ToStringBuilder OmitNullValues()
        {
            _omitNullValues = true;
            return this;
        }

        /// <summary>
        /// Add a value to the builder.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The key is null or empty.</exception>
        public ToStringBuilder Add<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Invalid ToStringBuilder key. Key should be non empty", nameof(key));
            }

            if (_values == null)
            {
                _values = new LinkedList<KeyValuePair<string, object>>();
            }
            _values.AddLast(new KeyValuePair<string, object>(key, value));
            return this;
        }

        /// <summary>
        /// Return a string specified by this builder.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(_className);
            stringBuilder.Append('{');
            if (_values != null)
            {
                bool firstValue = true;
                foreach (KeyValuePair<string,object> keyValuePair in _values)
                {
                    if (firstValue)
                    {
                        firstValue = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }
                    stringBuilder.Append(keyValuePair.Key).Append('=');
                    object value = keyValuePair.Value;
                    if (value == null)
                    {
                        if (!_omitNullValues)
                        {
                            stringBuilder.Append("null");
                        }
                    }
                    else
                    {
                        stringBuilder.Append(value);
                    }
                }
            }
            stringBuilder.Append('}');
            return stringBuilder.ToString();
        }
    }
}
