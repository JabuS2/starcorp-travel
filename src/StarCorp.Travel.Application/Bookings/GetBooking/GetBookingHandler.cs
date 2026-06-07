namespace StarCorp.Travel.Application.Bookings.GetBooking;
using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Application.Common.Exceptions;

public class GetBookingHandler
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingResponse> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Reserva {id} não encontrada");

        return new BookingResponse(
            booking.Id,
            booking.CustomerId,
            booking.FlightId,
            booking.BookingClass,
            booking.Status,
            booking.TotalAmount,
            booking.Passengers.Select(p => new PassengerResponse(p.Name, p.Document)).ToList());
    }
}
