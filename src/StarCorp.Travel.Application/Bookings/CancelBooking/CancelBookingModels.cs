namespace StarCorp.Travel.Application.Bookings.CancelBooking;
using StarCorp.Travel.Domain.Shared;

public record CancelBookingResponse(
    Guid BookingId,
    BookingStatus Status,
    decimal RefundPercentage,
    decimal RefundAmount);
