using System;

namespace ZionCodes.Core.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTimeOffset GetCurrentDateTime()
            => DateTimeOffset.UtcNow;
    }
}
