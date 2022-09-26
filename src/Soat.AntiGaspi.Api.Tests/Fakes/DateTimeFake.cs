using Soat.AntiGaspi.Api.Time;
using System;

namespace Soat.AntiGaspi.Api.Tests.Fakes
{
    public class DateOnlyFake : IDateOnly
    {
        private DateTime? _now;
        public DateTime Now 
        {
            get => _now ?? DateTime.UtcNow;
            set => _now = value;
        }
    }
}
