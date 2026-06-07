using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Domain.Bookings;

namespace StarCorp.Travel.Application.Tests.Fakes;

public class FakeBookingRepository : IBookingRepository
{
    public readonly Dictionary<Guid, Booking> Bookings = new();
    public readonly Dictionary<Guid, Payment> PaymentsByBooking = new();

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        Bookings[booking.Id] = booking;
        return Task.CompletedTask;
    }

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Bookings.TryGetValue(id, out var booking);
        return Task.FromResult(booking);
    }

    public Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        Bookings[booking.Id] = booking;
        return Task.CompletedTask;
    }

    public Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        PaymentsByBooking[payment.BookingId] = payment;
        return Task.CompletedTask;
    }

    public Task<Payment?> GetPaymentByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        PaymentsByBooking.TryGetValue(bookingId, out var payment);
        return Task.FromResult(payment);
    }
}
