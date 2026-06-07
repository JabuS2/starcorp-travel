namespace StarCorp.Travel.Application.Bookings;
using StarCorp.Travel.Domain.Shared;

public record PassengerResponse(string Name, string Document);

public record PriceBreakdownResponse(
    decimal Subtotal,
    decimal Taxes,
    decimal ServiceFee,
    decimal Total);

public record BookingResponse(
    Guid Id,
    Guid CustomerId,
    Guid FlightId,
    BookingClass BookingClass,
    BookingStatus Status,
    decimal TotalAmount,
    IReadOnlyList<PassengerResponse> Passengers);
