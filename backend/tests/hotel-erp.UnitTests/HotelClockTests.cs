using hotel_erp.Api.Services;

namespace hotel_erp.UnitTests;

public class HotelClockTests
{
    [Fact]
    public void Clock_ConvertsUtcToHondurasAndExposesLocalDate()
    {
        var provider = new FixedTimeProvider(new DateTimeOffset(2026, 9, 27, 3, 30, 0, TimeSpan.Zero));
        var clock = new HotelClock(provider);

        Assert.Equal(new DateTime(2026, 9, 27, 3, 30, 0, DateTimeKind.Utc), clock.UtcNow);
        Assert.Equal(new DateTime(2026, 9, 26, 21, 30, 0, DateTimeKind.Unspecified), clock.HondurasNow);
        Assert.Equal(new DateOnly(2026, 9, 26), clock.Today);
    }

    private sealed class FixedTimeProvider(DateTimeOffset value) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => value;
    }
}
