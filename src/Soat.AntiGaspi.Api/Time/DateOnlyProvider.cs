namespace Soat.AntiGaspi.Api.Time
{
    public class DateOnlyProvider : IDateOnly
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
