using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Domain.Tests.Bookings;

public class BookingTests
{
    private readonly Guid _customerId = Guid.NewGuid();
    private readonly Guid _flightId = Guid.NewGuid();
    private readonly BookingClass _bookingClass = BookingClass.Economy;
    private readonly decimal _totalAmount = 1500m;

    private static List<Passenger> ValidPassengers() => new()
    {
        new Passenger("John Doe", "47299303809")
    };

    [Fact]
    public void DeveCriarReservaComDadosValidos()
    {
        var booking = new Booking(_customerId, _flightId, _bookingClass, _totalAmount, ValidPassengers());

        Assert.NotEqual(Guid.Empty, booking.Id);
        Assert.Equal(_customerId, booking.CustomerId);
        Assert.Equal(_flightId, booking.FlightId);
        Assert.Equal(_bookingClass, booking.BookingClass);
        Assert.Equal(_totalAmount, booking.TotalAmount);
        Assert.Single(booking.Passengers);
    }

    [Fact]
    public void DeveNascerComStatusPendente()
    {
        var booking = new Booking(_customerId, _flightId, _bookingClass, _totalAmount, ValidPassengers());

        Assert.Equal(BookingStatus.Pending, booking.Status);
    }

    [Fact]
    public void DeveFalharAoCriarReservaSemPassageiros()
    {
        Assert.Throws<ArgumentException>(() => new Booking(_customerId, _flightId, _bookingClass, _totalAmount, new List<Passenger>()));
    }

    [Fact]
    public void DeveFalharAoCriarReservaComCustomerIdVazio()
    {
        Assert.Throws<ArgumentException>(() => new Booking(Guid.Empty, _flightId, _bookingClass, _totalAmount, ValidPassengers()));
    }

    [Fact]
    public void DeveFalharAoCriarReservaComFlightIdVazio()
    {
        Assert.Throws<ArgumentException>(() => new Booking(_customerId, Guid.Empty, _bookingClass, _totalAmount, ValidPassengers()));
    }

    [Fact]
    public void DeveFalharAoCriarReservaComTotalAmountInvalido()
    {
        Assert.Throws<ArgumentException>(() => new Booking(_customerId, _flightId, _bookingClass, 0m, ValidPassengers()));
    }

    [Fact]
    public void DeveConfirmarReservaPendente()
    {
        var booking = new Booking(_customerId, _flightId, _bookingClass, _totalAmount, ValidPassengers());

        booking.Confirm();

        Assert.Equal(BookingStatus.Confirmed, booking.Status);
    }

    [Fact]
    public void DeveFalharAoConfirmarReservaJaConfirmada()
    {
        var booking = new Booking(_customerId, _flightId, _bookingClass, _totalAmount, ValidPassengers());
        booking.Confirm();

        Assert.Throws<InvalidOperationException>(() => booking.Confirm());
    }

    [Fact]
    public void DeveFalharAoConfirmarReservaCancelada()
    {
        var booking = new Booking(_customerId, _flightId, _bookingClass, _totalAmount, ValidPassengers());
        booking.Cancel();

        Assert.Throws<InvalidOperationException>(() => booking.Confirm());
    }

    [Fact]
    public void DeveCancelarReservaPendente()
    {
        var booking = new Booking(_customerId, _flightId, _bookingClass, _totalAmount, ValidPassengers());

        booking.Cancel();

        Assert.Equal(BookingStatus.Cancelled, booking.Status);
    }

    [Fact]
    public void DeveCancelarReservaConfirmada()
    {
        var booking = new Booking(_customerId, _flightId, _bookingClass, _totalAmount, ValidPassengers());
        booking.Confirm();

        booking.Cancel();

        Assert.Equal(BookingStatus.Cancelled, booking.Status);
    }

    [Fact]
    public void DeveFalharAoCancelarReservaJaCancelada()
    {
        var booking = new Booking(_customerId, _flightId, _bookingClass, _totalAmount, ValidPassengers());
        booking.Cancel();

        Assert.Throws<InvalidOperationException>(() => booking.Cancel());
    }

    [Fact]
    public void DeveReconstruirReservaPreservandoIdEStatus()
    {
        var id = Guid.NewGuid();
        var createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var updatedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc);

        var booking = Booking.Restore(id, _customerId, _flightId, BookingClass.Business, BookingStatus.Confirmed, _totalAmount, ValidPassengers(), createdAt, updatedAt);

        Assert.Equal(id, booking.Id);
        Assert.Equal(BookingStatus.Confirmed, booking.Status);
        Assert.Equal(BookingClass.Business, booking.BookingClass);
        Assert.Equal(createdAt, booking.CreatedAt);
        Assert.Equal(updatedAt, booking.UpdatedAt);
    }
}
