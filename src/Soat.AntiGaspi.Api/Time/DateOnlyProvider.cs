namespace Soat.AntiGaspi.Api.Time
{
    public class DateOnlyProvider : IDateOnly
    {
        public DateOnly Now => DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
