using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Application.Common;
using StarCorp.Travel.Domain.Flights;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Application.Tests.Fakes;

public class FakeFlightRepository : IFlightRepository
{
    public readonly List<Flight> Flights = new();

    public Task<Flight?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var flight = Flights.FirstOrDefault(f => f.Id == id);
        return Task.FromResult(flight);
    }

    public Task<PagedResult<Flight>> SearchAsync(FlightSearchCriteria criteria, CancellationToken cancellationToken = default)
    {
        IEnumerable<Flight> query = Flights;

        if (!string.IsNullOrWhiteSpace(criteria.Origin))
            query = query.Where(f => f.Origin == criteria.Origin);

        if (!string.IsNullOrWhiteSpace(criteria.Destination))
            query = query.Where(f => f.Destination == criteria.Destination);

        if (criteria.Date.HasValue)
            query = query.Where(f => f.DepartureTime.Date == criteria.Date.Value.Date);

        if (criteria.MinPrice.HasValue)
            query = query.Where(f => f.BasePrice >= criteria.MinPrice.Value);

        if (criteria.MaxPrice.HasValue)
            query = query.Where(f => f.BasePrice <= criteria.MaxPrice.Value);

        if (criteria.BookingClass.HasValue)
            query = query.Where(f => criteria.BookingClass.Value == BookingClass.Business ? f.BusinessSeats > 0 : f.EconomySeats > 0);

        var all = query.ToList();
        var items = all
            .Skip((criteria.Page - 1) * criteria.PageSize)
            .Take(criteria.PageSize)
            .ToList();

        return Task.FromResult(new PagedResult<Flight>(items, criteria.Page, criteria.PageSize, all.Count));
    }
}
