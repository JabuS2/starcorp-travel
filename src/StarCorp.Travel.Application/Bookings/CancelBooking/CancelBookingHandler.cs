namespace StarCorp.Travel.Application.Bookings.CancelBooking;
using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Application.Common.Exceptions;
using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

public class CancelBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CancelBookingHandler(
        IBookingRepository bookingRepository,
        IFlightRepository flightRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _flightRepository = flightRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CancelBookingResponse> HandleAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken)
            ?? throw new NotFoundException($"Reserva {bookingId} não encontrada");

        if (booking.Status == BookingStatus.Cancelled)
            throw new ConflictException("A reserva já está cancelada");

        var flight = await _flightRepository.GetByIdAsync(booking.FlightId, cancellationToken)
            ?? throw new NotFoundException($"Voo {booking.FlightId} não encontrado");

        var payment = await _bookingRepository.GetPaymentByBookingIdAsync(bookingId, cancellationToken);

        var refund = CancellationService.CalculateRefund(
            booking.BookingClass,
            booking.TotalAmount,
            flight.DepartureTime,
            payment?.PaidAt,
            _dateTimeProvider.UtcNow);

        booking.Cancel();

        await _bookingRepository.UpdateAsync(booking, cancellationToken);

        return new CancelBookingResponse(booking.Id, booking.Status, refund.Percentage, refund.Amount);
    }
}
