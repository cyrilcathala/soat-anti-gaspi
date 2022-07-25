namespace Soat.AntiGaspi.Api.Time
{
    public class DateTimeOffsetProvider : IDateTimeOffset
    {
        public DateTimeOffset Now => DateTimeOffset.UtcNow;
    }
}
