using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Domain.Tests.Bookings;


public class PaymentTests
{
    private readonly Guid _paymentId = Guid.NewGuid();
    private readonly Guid _bookingId = Guid.NewGuid();
    private readonly Guid _customerId = Guid.NewGuid();
    private readonly decimal _amount = 1000m;
    private readonly PaymentMethod _paymentMethod = PaymentMethod.CreditCard;
    private readonly PaymentStatus _status = PaymentStatus.Pending;

    [Fact]
    public void DeveCriarPagamentoComDadosValidos()
    {
        var payment = new Payment(_bookingId, _customerId, _amount, _paymentMethod);

        Assert.NotEqual(Guid.Empty, payment.Id);
        Assert.Equal(_bookingId, payment.BookingId);
        Assert.Equal(_customerId, payment.CustomerId);
        Assert.Equal(_amount, payment.Amount);
        Assert.Equal(_paymentMethod, payment.PaymentMethod);
        Assert.Equal(_status, payment.Status);
    }

    [Fact]
    public void DeveNascerComStatusPendente()
    {
        var payment = new Payment(_bookingId, _customerId, _amount, _paymentMethod);

        Assert.Equal(PaymentStatus.Pending, payment.Status);
    }
    [Fact]
    public void DeveNascerComPaidAtNulo()
    {
        var payment = new Payment(_bookingId, _customerId, _amount, _paymentMethod);

        Assert.Null(payment.PaidAt);
    }
    [Fact]
    public void DeveConfirmarPagamentoEAtualizarStatusEData()
    {
        var payment = new Payment(_bookingId, _customerId, _amount, _paymentMethod);
        var confirmDate = DateTime.UtcNow;

        payment.Confirm(confirmDate);

        Assert.Equal(PaymentStatus.Confirmed, payment.Status);
        Assert.Equal(confirmDate, payment.PaidAt);
    }
    [Fact]
    public void DeveLancarExcecaoQuandoBookingIdEstiverVazio()
    {
        Assert.Throws<ArgumentException>(() => new Payment(Guid.Empty, _customerId, _amount, _paymentMethod));
    }
    [Fact]
    public void DeveLancarExcecaoQuandoCustomerIdEstiverVazio()
    {
        Assert.Throws<ArgumentException>(() => new Payment(_bookingId, Guid.Empty, _amount, _paymentMethod));
    }
    [Fact]
    public void DeveLancarExcecaoQuandoAmountForNegativo()
    {
        Assert.Throws<ArgumentException>(() => new Payment(_bookingId, _customerId, -100m, _paymentMethod));
    }


}