namespace StarCorp.Travel.Application.Flights.SearchFlights;
using StarCorp.Travel.Domain.Shared;

public record SearchFlightsRequest(
    string? Origin = null,
    string? Destination = null,
    DateTime? Date = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    BookingClass? BookingClass = null,
    int Page = 1,
    int PageSize = 10);

public record FlightResponse(
    Guid Id,
    string Origin,
    string Destination,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    decimal BasePrice,
    int EconomySeats,
    int BusinessSeats,
    Guid AirlineId);
