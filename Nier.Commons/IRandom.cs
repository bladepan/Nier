namespace Nier.Commons
{
    /// <summary>
    /// Abstract random generator operations.
    /// </summary>
    public interface IRandom
    {
        /// <summary>Returns a non-negative random integer.</summary>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="System.Int32.MaxValue"></see>.</returns>
        int Next();

        /// <summary>Returns a non-negative random integer that is less than the specified maximum.</summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to 0.</param>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than <paramref name="maxValue">maxValue</paramref>; that is, the range of return values ordinarily includes 0 but not <paramref name="maxValue">maxValue</paramref>. However, if <paramref name="maxValue">maxValue</paramref> equals 0, <paramref name="maxValue">maxValue</paramref> is returned.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="maxValue">maxValue</paramref> is less than 0.</exception>
        int Next(int maxValue);

        /// <summary>Returns a random integer that is within a specified range.</summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to <paramref name="minValue">minValue</paramref> and less than <paramref name="maxValue">maxValue</paramref>; that is, the range of return values includes <paramref name="minValue">minValue</paramref> but not <paramref name="maxValue">maxValue</paramref>. If <paramref name="minValue">minValue</paramref> equals <paramref name="maxValue">maxValue</paramref>, <paramref name="minValue">minValue</paramref> is returned.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="minValue">minValue</paramref> is greater than <paramref name="maxValue">maxValue</paramref>.</exception>
        int Next(int minValue, int maxValue);

        /// <summary>Fills the elements of a specified array of bytes with random numbers.</summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
        void NextBytes(byte[] buffer);

        /// <summary>Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.</summary>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
        double NextDouble();
    }
}
