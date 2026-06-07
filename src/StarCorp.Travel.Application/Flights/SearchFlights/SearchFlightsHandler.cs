namespace StarCorp.Travel.Application.Flights.SearchFlights;
using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Application.Common;

public class SearchFlightsHandler
{
    private const int MaxPageSize = 100;
    private readonly IFlightRepository _flightRepository;

    public SearchFlightsHandler(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    public async Task<PagedResult<FlightResponse>> HandleAsync(SearchFlightsRequest request, CancellationToken cancellationToken = default)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 10 : Math.Min(request.PageSize, MaxPageSize);

        if (request.MinPrice.HasValue && request.MaxPrice.HasValue && request.MinPrice > request.MaxPrice)
            throw new ArgumentException("preço mínimo não pode ser maior que o preço máximo");

        var criteria = new FlightSearchCriteria(
            request.Origin,
            request.Destination,
            request.Date,
            request.MinPrice,
            request.MaxPrice,
            request.BookingClass,
            page,
            pageSize);

        var result = await _flightRepository.SearchAsync(criteria, cancellationToken);

        var items = result.Items
            .Select(f => new FlightResponse(
                f.Id,
                f.Origin,
                f.Destination,
                f.DepartureTime,
                f.ArrivalTime,
                f.BasePrice,
                f.EconomySeats,
                f.BusinessSeats,
                f.AirlineId))
            .ToList();

        return new PagedResult<FlightResponse>(items, result.Page, result.PageSize, result.TotalCount);
    }
}
