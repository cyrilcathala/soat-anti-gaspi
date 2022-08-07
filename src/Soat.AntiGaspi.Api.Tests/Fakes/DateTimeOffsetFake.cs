using Soat.AntiGaspi.Api.Time;
using System;

namespace Soat.AntiGaspi.Api.Tests.Fakes
{
    public class DateTimeOffsetFake : IDateTimeOffset
    {
        private DateTimeOffset? _now;
        public DateTimeOffset Now 
        {
            get => _now ?? DateTimeOffset.UtcNow;
            set => _now = value;
        }
    }
}
