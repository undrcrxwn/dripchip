namespace DripChip.Application.Extensions;

public static class DateTimeOffsetExtensions
{
    public static DateTimeOffset Trim(this DateTimeOffset dateTimeOffset, long roundTicks) =>
        new(dateTimeOffset.Ticks - dateTimeOffset.Ticks % roundTicks, TimeSpan.Zero);
}