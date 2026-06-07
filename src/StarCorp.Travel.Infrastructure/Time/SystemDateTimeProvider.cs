namespace StarCorp.Travel.Infrastructure.Time;
using StarCorp.Travel.Application.Abstractions;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
