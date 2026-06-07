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

    private readonly Guid _customerId;
    private readonly Flight _flight;

    public CreateBookingHandlerTests()
    {
        _handler = new CreateBookingHandler(_bookings, _flights, _customers);
        _customerId = _customers.Add();
        _flight = new Flight("São Paulo", "Rio de Janeiro", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(10).AddHours(2), 1000m, 100, 20, Guid.NewGuid());
        _flights.Flights.Add(_flight);
    }

    private CreateBookingRequest NovaRequest(Guid? customerId = null, Guid? flightId = null, BookingClass bookingClass = BookingClass.Economy, int passageiros = 1)
        => new(
            customerId ?? _customerId,
            flightId ?? _flight.Id,
            bookingClass,
            Enumerable.Range(0, passageiros).Select(_ => new PassengerRequest("John Doe", "47299303809")).ToList());

    [Fact]
    public async Task DeveCriarReservaComBreakdownDePrecoSemAjusteDePagamento()
    {
        var response = await _handler.HandleAsync(NovaRequest());

        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(BookingStatus.Pending, response.Status);
        Assert.Equal(response.PriceBreakdown.Total, response.TotalAmount);
        Assert.Equal(response.PriceBreakdown.Subtotal + response.PriceBreakdown.Taxes + response.PriceBreakdown.ServiceFee, response.PriceBreakdown.Total);
        Assert.True(_bookings.Bookings.ContainsKey(response.Id));
    }

    [Fact]
    public async Task DeveFalharQuandoClienteNaoExiste()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(NovaRequest(customerId: Guid.NewGuid())));
    }

    [Fact]
    public async Task DeveFalharQuandoClienteInativo()
    {
        var inativo = _customers.Add(isActive: false);

        await Assert.ThrowsAsync<ConflictException>(() => _handler.HandleAsync(NovaRequest(customerId: inativo)));
    }

    [Fact]
    public async Task DeveFalharQuandoVooNaoExiste()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(NovaRequest(flightId: Guid.NewGuid())));
    }

    [Fact]
    public async Task DeveFalharQuandoVooInativo()
    {
        var inativo = Flight.Restore(Guid.NewGuid(), "São Paulo", "Rio de Janeiro", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(10).AddHours(2), 1000m, 100, 20, Guid.NewGuid(), false, DateTime.UtcNow, DateTime.UtcNow);
        _flights.Flights.Add(inativo);

        await Assert.ThrowsAsync<ConflictException>(() => _handler.HandleAsync(NovaRequest(flightId: inativo.Id)));
    }

    [Fact]
    public async Task DeveFalharSemPassageiros()
    {
        var request = new CreateBookingRequest(_customerId, _flight.Id, BookingClass.Economy, new List<PassengerRequest>());

        await Assert.ThrowsAsync<ArgumentException>(() => _handler.HandleAsync(request));
    }

    [Fact]
    public async Task DeveFalharQuandoNaoHaAssentosSuficientes()
    {
        await Assert.ThrowsAsync<ConflictException>(() =>
            _handler.HandleAsync(NovaRequest(bookingClass: BookingClass.Business, passageiros: 21)));
    }
}
