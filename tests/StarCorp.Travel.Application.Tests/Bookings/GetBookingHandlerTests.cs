using StarCorp.Travel.Application.Bookings.GetBooking;
using StarCorp.Travel.Application.Common.Exceptions;
using StarCorp.Travel.Application.Tests.Fakes;
using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Application.Tests.Bookings;

public class GetBookingHandlerTests
{
    private readonly FakeBookingRepository _bookings = new();
    private readonly GetBookingHandler _handler;

    public GetBookingHandlerTests()
    {
        _handler = new GetBookingHandler(_bookings);
    }

    [Fact]
    public async Task DeveRetornarReservaExistente()
    {
        var booking = new Booking(Guid.NewGuid(), Guid.NewGuid(), BookingClass.Economy, 1000m,
            new[] { new Passenger("John Doe", "47299303809") });
        _bookings.Bookings[booking.Id] = booking;

        var response = await _handler.HandleAsync(booking.Id);

        Assert.Equal(booking.Id, response.Id);
        Assert.Single(response.Passengers);
    }

    [Fact]
    public async Task DeveFalharQuandoReservaNaoExiste()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(Guid.NewGuid()));
    }
}
