using System;

namespace Nier.Commons
{
    /// <summary>
    /// Implementation of <see cref="ISystemClock"/> that uses real system clock.
    /// </summary>
    public class SystemClock : ISystemClock
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static readonly SystemClock Instance = new ();

        private SystemClock()
        {
        }

        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
