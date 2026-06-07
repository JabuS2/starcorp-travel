using StarCorp.Travel.Application.Bookings.CreateBooking;
using StarCorp.Travel.Application.Common.Exceptions;
using StarCorp.Travel.Application.Tests.Fakes;
using StarCorp.Travel.Domain.Flights;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Application.Tests.Bookings;

public class CreateBookingHandlerTests
{
    private readonly FakeBookingRepository _bookings = new();
    private readonly FakeFlightRepository _flights = new();
    private readonly FakeCustomerRepository _customers = new();
    private readonly CreateBookingHandler _handler;

    private readonly Guid _customerId = Guid.NewGuid();
    private readonly Flight _flight;

    public CreateBookingHandlerTests()
    {
        _handler = new CreateBookingHandler(_bookings, _flights, _customers);
        _customers.ExistingCustomers.Add(_customerId);
        _flight = new Flight("São Paulo", "Rio de Janeiro", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(10).AddHours(2), 1000m, 100, 20, Guid.NewGuid());
        _flights.Flights.Add(_flight);
    }

    private CreateBookingRequest NovaRequest(BookingClass bookingClass = BookingClass.Economy, int passageiros = 1)
        => new(
            _customerId,
            _flight.Id,
            bookingClass,
            PaymentMethod.Pix,
            Enumerable.Range(0, passageiros).Select(_ => new PassengerRequest("John Doe", "47299303809")).ToList());

    [Fact]
    public async Task DeveCriarReservaComBreakdownDePreco()
    {
        var response = await _handler.HandleAsync(NovaRequest());

        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(BookingStatus.Pending, response.Status);
        Assert.Equal(response.PriceBreakdown.Total, response.TotalAmount);
        Assert.True(_bookings.Bookings.ContainsKey(response.Id));
    }

    [Fact]
    public async Task DeveFalharQuandoClienteNaoExiste()
    {
        var request = new CreateBookingRequest(Guid.NewGuid(), _flight.Id, BookingClass.Economy, PaymentMethod.Pix,
            new[] { new PassengerRequest("John Doe", "47299303809") });

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(request));
    }

    [Fact]
    public async Task DeveFalharQuandoVooNaoExiste()
    {
        var request = new CreateBookingRequest(_customerId, Guid.NewGuid(), BookingClass.Economy, PaymentMethod.Pix,
            new[] { new PassengerRequest("John Doe", "47299303809") });

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(request));
    }

    [Fact]
    public async Task DeveFalharSemPassageiros()
    {
        var request = new CreateBookingRequest(_customerId, _flight.Id, BookingClass.Economy, PaymentMethod.Pix,
            new List<PassengerRequest>());

        await Assert.ThrowsAsync<ArgumentException>(() => _handler.HandleAsync(request));
    }

    [Fact]
    public async Task DeveFalharQuandoNaoHaAssentosSuficientes()
    {
        await Assert.ThrowsAsync<ConflictException>(() =>
            _handler.HandleAsync(NovaRequest(BookingClass.Business, passageiros: 21)));
    }
}
