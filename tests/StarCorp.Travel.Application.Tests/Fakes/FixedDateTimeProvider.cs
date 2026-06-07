using StarCorp.Travel.Application.Abstractions;

namespace StarCorp.Travel.Application.Tests.Fakes;

public class FixedDateTimeProvider : IDateTimeProvider
{
    public FixedDateTimeProvider(DateTime utcNow)
    {
        UtcNow = utcNow;
    }

    public DateTime UtcNow { get; }
}
