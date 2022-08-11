using Soat.AntiGaspi.Api.Time;
using System;

namespace Soat.AntiGaspi.Api.Tests.Fakes
{
    public class DateOnlyFake : IDateOnly
    {
        private DateOnly? _now;
        public DateOnly Now 
        {
            get => _now ?? DateOnly.FromDateTime(DateTime.UtcNow);
            set => _now = value;
        }
    }
}
