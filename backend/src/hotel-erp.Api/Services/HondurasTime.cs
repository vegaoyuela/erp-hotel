namespace hotel_erp.Api.Services
{
    public interface IHotelClock
    {
        DateTime UtcNow { get; }
        DateTime HondurasNow { get; }
        DateOnly Today { get; }
    }

    public sealed class HotelClock(TimeProvider timeProvider) : IHotelClock
    {
        private static readonly TimeZoneInfo HondurasTimeZone = FindHondurasTimeZone();

        public DateTime UtcNow => timeProvider.GetUtcNow().UtcDateTime;

        public DateTime HondurasNow => TimeZoneInfo.ConvertTimeFromUtc(UtcNow, HondurasTimeZone);

        public DateOnly Today => DateOnly.FromDateTime(HondurasNow);

        private static TimeZoneInfo FindHondurasTimeZone()
        {
            foreach (var id in new[] { "America/Tegucigalpa", "Central America Standard Time" })
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(id);
                }
                catch (TimeZoneNotFoundException)
                {
                }
            }

            throw new InvalidOperationException("No se encontró la zona horaria de Honduras en el sistema operativo.");
        }
    }

}
