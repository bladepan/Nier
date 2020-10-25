using System;

namespace Nier.TwoB.Benchmarks
{
    public static class StringGenerator
    {
        private static readonly Random s_random = new Random(42);

        public static string GenString(int length)
        {
            byte[] bytes = new byte[length];
            s_random.NextBytes(bytes);
            string str = Convert.ToBase64String(bytes);
            return str.Substring(0, length);
        }
    }
}
