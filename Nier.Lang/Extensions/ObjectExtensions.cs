using System;
using System.Collections.Generic;
using System.Text;

namespace Nier.Lang.Extensions
{
    public static class ObjectExtensions
    {
        public static ToStringBuilder ToStringBuilder(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            return new ToStringBuilder(obj.GetType().Name);
        }

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
    /// NotThreadSafe
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

        public ToStringBuilder OmitNullValues()
        {
            _omitNullValues = true;
            return this;
        }

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
