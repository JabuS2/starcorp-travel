using StarCorp.Travel.Application.Bookings.CancelBooking;
using StarCorp.Travel.Application.Common.Exceptions;
using StarCorp.Travel.Application.Tests.Fakes;
using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Flights;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Application.Tests.Bookings;

public class CancelBookingHandlerTests
{
    private readonly FakeBookingRepository _bookings = new();
    private readonly FakeFlightRepository _flights = new();
    private readonly FixedDateTimeProvider _clock = new(new DateTime(2026, 6, 7, 12, 0, 0, DateTimeKind.Utc));
    private readonly CancelBookingHandler _handler;

    public CancelBookingHandlerTests()
    {
        _handler = new CancelBookingHandler(_bookings, _flights, _clock);
    }

    private Booking NovaReserva(BookingClass bookingClass, DateTime departure)
    {
        var flight = new Flight("São Paulo", "Rio de Janeiro", departure, departure.AddHours(2), 1000m, 100, 20, Guid.NewGuid());
        _flights.Flights.Add(flight);

        var booking = new Booking(Guid.NewGuid(), flight.Id, bookingClass, 1000m,
            new[] { new Passenger("John Doe", "47299303809") });
        _bookings.Bookings[booking.Id] = booking;
        return booking;
    }

    [Fact]
    public async Task DeveCancelarComReembolsoTotalParaMaisDeSeteDias()
    {
        var booking = NovaReserva(BookingClass.Economy, _clock.UtcNow.AddDays(10));

        var response = await _handler.HandleAsync(booking.Id);

        Assert.Equal(BookingStatus.Cancelled, response.Status);
        Assert.Equal(1.0m, response.RefundPercentage);
        Assert.Equal(1000m, response.RefundAmount);
    }

    [Fact]
    public async Task DeveCancelarComReembolsoParcialEconomy()
    {
        var booking = NovaReserva(BookingClass.Economy, _clock.UtcNow.AddDays(5));

        var response = await _handler.HandleAsync(booking.Id);

        Assert.Equal(0.50m, response.RefundPercentage);
        Assert.Equal(500m, response.RefundAmount);
    }

    [Fact]
    public async Task DeveFalharQuandoReservaNaoExiste()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task DeveFalharQuandoReservaJaCancelada()
    {
        var booking = NovaReserva(BookingClass.Economy, _clock.UtcNow.AddDays(10));
        booking.Cancel();

        await Assert.ThrowsAsync<ConflictException>(() => _handler.HandleAsync(booking.Id));
    }
}
