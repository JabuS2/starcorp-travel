using StarCorp.Travel.Application.Bookings.ProcessPayment;
using StarCorp.Travel.Application.Common.Exceptions;
using StarCorp.Travel.Application.Tests.Fakes;
using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Application.Tests.Bookings;

public class ProcessPaymentHandlerTests
{
    private readonly FakeBookingRepository _bookings = new();
    private readonly FixedDateTimeProvider _clock = new(new DateTime(2026, 6, 7, 12, 0, 0, DateTimeKind.Utc));
    private readonly ProcessPaymentHandler _handler;

    public ProcessPaymentHandlerTests()
    {
        _handler = new ProcessPaymentHandler(_bookings, _clock);
    }

    private Booking NovaReserva()
    {
        var booking = new Booking(Guid.NewGuid(), Guid.NewGuid(), BookingClass.Economy, 1000m,
            new[] { new Passenger("John Doe", "47299303809") });
        _bookings.Bookings[booking.Id] = booking;
        return booking;
    }

    [Fact]
    public async Task DeveProcessarPagamentoComAjusteDoCartaoEConfirmarReserva()
    {
        var booking = NovaReserva();

        var response = await _handler.HandleAsync(booking.Id, new ProcessPaymentRequest(PaymentMethod.CreditCard));

        Assert.Equal(PaymentStatus.Confirmed, response.PaymentStatus);
        Assert.Equal(BookingStatus.Confirmed, response.BookingStatus);
        Assert.Equal(1000m, response.BaseAmount);
        Assert.Equal(30m, response.PaymentAdjustment);
        Assert.Equal(1030m, response.Amount);
        Assert.Equal(_clock.UtcNow, response.PaidAt);
    }

    [Fact]
    public async Task DeveAplicarDescontoDoPixEAtualizarTotalDaReserva()
    {
        var booking = NovaReserva();

        var response = await _handler.HandleAsync(booking.Id, new ProcessPaymentRequest(PaymentMethod.Pix));

        Assert.Equal(950m, response.Amount);
        Assert.Equal(950m, _bookings.Bookings[booking.Id].TotalAmount);
    }

    [Fact]
    public async Task DeveFalharQuandoReservaNaoExiste()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.HandleAsync(Guid.NewGuid(), new ProcessPaymentRequest(PaymentMethod.Pix)));
    }

    [Fact]
    public async Task DeveFalharQuandoReservaJaPaga()
    {
        var booking = NovaReserva();
        await _handler.HandleAsync(booking.Id, new ProcessPaymentRequest(PaymentMethod.Pix));

        await Assert.ThrowsAsync<ConflictException>(() =>
            _handler.HandleAsync(booking.Id, new ProcessPaymentRequest(PaymentMethod.Pix)));
    }

    [Fact]
    public async Task DeveFalharQuandoReservaCancelada()
    {
        var booking = NovaReserva();
        booking.Cancel();

        await Assert.ThrowsAsync<ConflictException>(() =>
            _handler.HandleAsync(booking.Id, new ProcessPaymentRequest(PaymentMethod.Pix)));
    }
}
