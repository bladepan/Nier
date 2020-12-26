using System;
using System.Text;

namespace Nier.Commons.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        /// create a random string of specified length with characters sampled from specified characters array.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="characters"></param>
        /// <param name="length"></param>
        /// <returns>a random string</returns>
        /// <exception cref="ArgumentNullException">when random or characters is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">when length is negative</exception>
        /// <exception cref="ArgumentException">when characters is empty</exception>
        public static string RandomString(this IRandom random, char[] characters, int length)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (characters == null)
            {
                throw new ArgumentNullException(nameof(characters));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, "string length must be greater than 0");
            }

            int charactersLength = characters.Length;
            if (charactersLength == 0)
            {
                throw new ArgumentException($"{characters} cannot be empty", nameof(characters));
            }

            if (length == 0)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(characters[random.Next(charactersLength)]);
            }

            return stringBuilder.ToString();
        }
    }
}
