using System;
using System.Security.Cryptography;

namespace Nier.Commons
{
    /// <summary>
    /// Random implementation based on <see cref="RNGCryptoServiceProvider"/>.
    /// </summary>
    public class RNGCryptoRandom : IRandom
    {
        private static readonly RNGCryptoServiceProvider s_globalRandomProvider = new RNGCryptoServiceProvider();
        public static RNGCryptoRandom Instance = new RNGCryptoRandom();

        private RNGCryptoRandom()
        {
        }

        public int Next()
        {
            byte[] buffer = new byte[4];
            s_globalRandomProvider.GetBytes(buffer);
            int result = BitConverter.ToInt32(buffer, 0);
            return Math.Abs(result);
        }

        public int Next(int maxValue)
        {
            if (maxValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), maxValue,
                    $"'{nameof(maxValue)}' must be greater than zero.");
            }

            if (maxValue <= 1)
            {
                return 0;
            }

            return Next() % maxValue;
        }

        public int Next(int minValue, int maxValue)
        {
            if (maxValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), maxValue,
                    $"'{nameof(maxValue)}' must be greater than zero.");
            }

            if (minValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), minValue,
                    $"'{nameof(minValue)}' must be greater than zero.");
            }

            if (minValue == maxValue)
            {
                return minValue;
            }

            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), minValue,
                    $"'{nameof(minValue)}' cannot be greater than {maxValue}.");
            }

            return minValue + Next(maxValue - minValue);
        }

        public void NextBytes(byte[] buffer) => s_globalRandomProvider.GetBytes(buffer);

        public double NextDouble()
        {
            byte[] buffer = new byte[8];

            double result;
            do
            {
                s_globalRandomProvider.GetBytes(buffer);
                result = BitConverter.ToDouble(buffer, 0);
                if (result >= 0.0 && result < 1.0)
                {
                    return result;
                }

                result = Math.Abs(result);
                double intPart = Math.Truncate(result);
                result = result - intPart;
            } while (!(result >= 0.0 && result < 1.0));

            return result;
        }
    }
}
