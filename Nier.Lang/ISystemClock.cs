using System;

namespace Nier.Lang
{
    /// <summary>
    /// Abstracts the system clock to facilitate testing.
    /// </summary>
    public interface ISystemClock
    {
        /// <summary>
        /// Retrieves the current system time in UTC.
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}
