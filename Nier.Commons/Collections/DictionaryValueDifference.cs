using System.Collections.Generic;

namespace Nier.Commons.Collections
{
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
