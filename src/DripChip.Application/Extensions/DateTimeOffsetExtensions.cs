namespace DripChip.Application.Extensions;

public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Trims (rounds) the specified date-time the specified precision in ticks.
    /// </summary>
    /// <example>Passing round ticks precision of TimeSpan.TicksPerSecond gets 11:58:27.2953073 trimmed to 11:58:27.</example>
    /// <param name="dateTimeOffset">Date-time to be trimmed.</param>
    /// <param name="roundTicks">Trimming precision in ticks.</param>
    /// <returns>Trimmed date-time.</returns>
    public static DateTimeOffset Trim(this DateTimeOffset dateTimeOffset, long roundTicks) =>
        new(dateTimeOffset.Ticks - dateTimeOffset.Ticks % roundTicks, TimeSpan.Zero);
}