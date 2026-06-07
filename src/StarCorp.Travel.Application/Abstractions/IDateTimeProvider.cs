namespace StarCorp.Travel.Application.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
