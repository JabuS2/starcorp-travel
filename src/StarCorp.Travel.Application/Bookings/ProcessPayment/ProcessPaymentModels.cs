namespace StarCorp.Travel.Application.Bookings.ProcessPayment;
using StarCorp.Travel.Domain.Shared;

public record ProcessPaymentRequest(PaymentMethod PaymentMethod);

public record ProcessPaymentResponse(
    Guid PaymentId,
    Guid BookingId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus,
    DateTime? PaidAt,
    BookingStatus BookingStatus);
