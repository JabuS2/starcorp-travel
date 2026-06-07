namespace StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Application.Common;
using StarCorp.Travel.Domain.Flights;
using StarCorp.Travel.Domain.Shared;

public record FlightSearchCriteria(
    string? Origin,
    string? Destination,
    DateTime? Date,
    decimal? MinPrice,
    decimal? MaxPrice,
    BookingClass? BookingClass,
    int Page,
    int PageSize);

public interface IFlightRepository
{
    Task<PagedResult<Flight>> SearchAsync(FlightSearchCriteria criteria, CancellationToken cancellationToken = default);
    Task<Flight?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
