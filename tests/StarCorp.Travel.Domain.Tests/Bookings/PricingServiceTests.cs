using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Domain.Tests.Bookings;

public class PricingServiceTests
{
    private readonly decimal _basePrice = 1000m;

    [Fact]
    public void DeveCalcularPrecoEconomyComCartaoDeCredito()
    {
        var breakdown = PricingService.Calculate(_basePrice, 2, BookingClass.Economy, PaymentMethod.CreditCard);

        Assert.Equal(2000m, breakdown.Subtotal);
        Assert.Equal(250m, breakdown.Taxes);
        Assert.Equal(112.50m, breakdown.ServiceFee);
        Assert.Equal(70.88m, breakdown.PaymentAdjustment);
        Assert.Equal(2433.38m, breakdown.Total);
    }

    [Fact]
    public void DeveAplicarDescontoNoPix()
    {
        var breakdown = PricingService.Calculate(_basePrice, 1, BookingClass.Economy, PaymentMethod.Pix);

        Assert.True(breakdown.PaymentAdjustment < 0);
    }

    [Fact]
    public void DeveAplicarAcrescimoNoBoleto()
    {
        var breakdown = PricingService.Calculate(_basePrice, 1, BookingClass.Economy, PaymentMethod.Boleto);

        Assert.True(breakdown.PaymentAdjustment > 0);
    }

    [Fact]
    public void DeveAplicarMultiplicadorBusiness()
    {
        var economy = PricingService.Calculate(_basePrice, 1, BookingClass.Economy, PaymentMethod.Pix);
        var business = PricingService.Calculate(_basePrice, 1, BookingClass.Business, PaymentMethod.Pix);

        Assert.True(business.Subtotal > economy.Subtotal);
    }

    [Fact]
    public void TotalDeveSerASomaDosComponentes()
    {
        var breakdown = PricingService.Calculate(_basePrice, 3, BookingClass.Business, PaymentMethod.Boleto);

        var soma = breakdown.Subtotal + breakdown.Taxes + breakdown.ServiceFee + breakdown.PaymentAdjustment;
        Assert.Equal(breakdown.Total, soma);
    }

    [Fact]
    public void DeveFalharComQuantidadeDePassageirosInvalida()
    {
        Assert.Throws<ArgumentException>(() => PricingService.Calculate(_basePrice, 0, BookingClass.Economy, PaymentMethod.Pix));
    }

    [Fact]
    public void DeveFalharComPrecoBaseInvalido()
    {
        Assert.Throws<ArgumentException>(() => PricingService.Calculate(0m, 1, BookingClass.Economy, PaymentMethod.Pix));
    }
}
