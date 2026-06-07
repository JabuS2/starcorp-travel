namespace StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Domain.Bookings;

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default);
    Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<Payment?> GetPaymentByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default);
}
