using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Domain.Tests.Bookings;

public class PricingServiceTests
{
    private readonly decimal _basePrice = 1000m;

    [Fact]
    public void DeveCalcularPrecoBaseEconomy()
    {
        var breakdown = PricingService.CalculateBase(_basePrice, 2, BookingClass.Economy);

        Assert.Equal(2000m, breakdown.Subtotal);
        Assert.Equal(250m, breakdown.Taxes);
        Assert.Equal(112.50m, breakdown.ServiceFee);
        Assert.Equal(2362.50m, breakdown.Total);
    }

    [Fact]
    public void TotalBaseDeveSerASomaDosComponentes()
    {
        var breakdown = PricingService.CalculateBase(_basePrice, 3, BookingClass.Business);

        Assert.Equal(breakdown.Total, breakdown.Subtotal + breakdown.Taxes + breakdown.ServiceFee);
    }

    [Fact]
    public void DeveAplicarMultiplicadorBusiness()
    {
        var economy = PricingService.CalculateBase(_basePrice, 1, BookingClass.Economy);
        var business = PricingService.CalculateBase(_basePrice, 1, BookingClass.Business);

        Assert.Equal(business.Subtotal, economy.Subtotal * 2.5m);
    }

    [Fact]
    public void DeveFalharComQuantidadeDePassageirosInvalida()
    {
        Assert.Throws<ArgumentException>(() => PricingService.CalculateBase(_basePrice, 0, BookingClass.Economy));
    }

    [Fact]
    public void DeveFalharComPrecoBaseInvalido()
    {
        Assert.Throws<ArgumentException>(() => PricingService.CalculateBase(0m, 1, BookingClass.Economy));
    }

    [Fact]
    public void DeveAplicarAcrescimoNoCartaoDeCredito()
    {
        var payment = PricingService.CalculatePayment(2362.50m, PaymentMethod.CreditCard);

        Assert.Equal(70.88m, payment.PaymentAdjustment);
        Assert.Equal(2433.38m, payment.Total);
    }

    [Fact]
    public void DeveAplicarDescontoNoPix()
    {
        var payment = PricingService.CalculatePayment(1000m, PaymentMethod.Pix);

        Assert.Equal(-50m, payment.PaymentAdjustment);
        Assert.Equal(950m, payment.Total);
    }

    [Fact]
    public void DeveAplicarAcrescimoNoBoleto()
    {
        var payment = PricingService.CalculatePayment(1000m, PaymentMethod.Boleto);

        Assert.Equal(10m, payment.PaymentAdjustment);
        Assert.Equal(1010m, payment.Total);
    }

    [Fact]
    public void DeveFalharComTotalBaseInvalido()
    {
        Assert.Throws<ArgumentException>(() => PricingService.CalculatePayment(0m, PaymentMethod.Pix));
    }
}
