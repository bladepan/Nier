using System.Collections.Generic;
using System.Linq;

namespace Nier.Commons.Collections
{
    /// <summary>
    /// Utility methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class Enumerables
    {
        /// <summary>
        /// A <see cref="IEqualityComparer{T}"/> that returns true when either condition meets
        /// 1. both enumerables are null
        /// 2. object.Equals returns true
        /// 3. both enumerables has same of objects and the objects appear in the same order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEqualityComparer<IEnumerable<T>> SequenceEqualityComparer<T>()
        {
            return new SequenceEqualityComparator<T>();
        }
    }

    internal class SequenceEqualityComparator<T> : IEqualityComparer<IEnumerable<T>>
    {
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (object.Equals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.SequenceEqual(y);
        }

        public int GetHashCode(IEnumerable<T> obj) => obj.GetHashCode();
    }
}
