using System;
using System.Security.Cryptography;

namespace Nier.Commons
{
    /// <summary>
    /// <seealso cref="System.Random"/> is not thread safe. This is a thread safe version random based on
    /// https://devblogs.microsoft.com/pfxteam/getting-random-numbers-in-a-thread-safe-way/.
    /// </summary>
    public class ThreadLocalRandom: IRandom
    {
        private static readonly RandomNumberGenerator s_globalRandomProvider = RandomNumberGenerator.Create();

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static readonly ThreadLocalRandom Instance = new ();

        [ThreadStatic] private static Random _local;

        /// <summary>
        /// get the random instance of current thread.
        /// </summary>
        /// <returns></returns>
        private static Random Current()
        {
            Random instance = _local;
            if (instance == null)
            {
                byte[] buffer = new byte[4];
                s_globalRandomProvider.GetBytes(buffer);
                _local = instance = new Random(BitConverter.ToInt32(buffer, 0));
            }

            return instance;
        }

        private ThreadLocalRandom(){}

        public int Next()
        {
            return Current().Next();
        }

        public int Next(int maxValue)
        {
            return Current().Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return Current().Next(minValue, maxValue);
        }

        public void NextBytes(byte[] buffer)
        {
            Current().NextBytes(buffer);
        }

        public double NextDouble()
        {
            return Current().NextDouble();
        }
    }
}
