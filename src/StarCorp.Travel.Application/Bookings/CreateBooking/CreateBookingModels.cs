namespace StarCorp.Travel.Application.Bookings.CreateBooking;
using StarCorp.Travel.Domain.Shared;

public record PassengerRequest(string Name, string Document);

public record CreateBookingRequest(
    Guid CustomerId,
    Guid FlightId,
    BookingClass BookingClass,
    IReadOnlyList<PassengerRequest> Passengers);

public record CreateBookingResponse(
    Guid Id,
    Guid CustomerId,
    Guid FlightId,
    BookingClass BookingClass,
    BookingStatus Status,
    decimal TotalAmount,
    IReadOnlyList<PassengerResponse> Passengers,
    PriceBreakdownResponse PriceBreakdown);
