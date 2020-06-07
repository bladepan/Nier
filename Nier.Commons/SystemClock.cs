using System;

namespace Nier.Commons
{
    /// <summary>
    /// Implementation of <see cref="ISystemClock"/> that uses real system clock.
    /// </summary>
    public class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
