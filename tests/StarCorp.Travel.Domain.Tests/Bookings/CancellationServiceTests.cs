using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Domain.Tests.Bookings;

public class CancellationServiceTests
{
    private readonly decimal _totalAmount = 1000m;
    private readonly DateTime _now = new(2026, 6, 7, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void DeveReembolsar100PorCentoEconomyComMaisDeSeteDias()
    {
        var result = CancellationService.CalculateRefund(BookingClass.Economy, _totalAmount, _now.AddDays(10), null, _now);

        Assert.Equal(1.0m, result.Percentage);
        Assert.Equal(1000m, result.Amount);
    }

    [Fact]
    public void DeveReembolsar100PorCentoBusinessComMaisDeSeteDias()
    {
        var result = CancellationService.CalculateRefund(BookingClass.Business, _totalAmount, _now.AddDays(10), null, _now);

        Assert.Equal(1.0m, result.Percentage);
    }

    [Fact]
    public void DeveReembolsar50PorCentoEconomyEntreDoisESeteDias()
    {
        var result = CancellationService.CalculateRefund(BookingClass.Economy, _totalAmount, _now.AddDays(5), null, _now);

        Assert.Equal(0.50m, result.Percentage);
        Assert.Equal(500m, result.Amount);
    }

    [Fact]
    public void DeveReembolsar75PorCentoBusinessEntreDoisESeteDias()
    {
        var result = CancellationService.CalculateRefund(BookingClass.Business, _totalAmount, _now.AddDays(5), null, _now);

        Assert.Equal(0.75m, result.Percentage);
        Assert.Equal(750m, result.Amount);
    }

    [Fact]
    public void DeveReembolsar0PorCentoEconomyComMenosDeDoisDias()
    {
        var result = CancellationService.CalculateRefund(BookingClass.Economy, _totalAmount, _now.AddDays(1), null, _now);

        Assert.Equal(0.0m, result.Percentage);
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void DeveReembolsar25PorCentoBusinessComMenosDeDoisDias()
    {
        var result = CancellationService.CalculateRefund(BookingClass.Business, _totalAmount, _now.AddDays(1), null, _now);

        Assert.Equal(0.25m, result.Percentage);
        Assert.Equal(250m, result.Amount);
    }

    [Fact]
    public void DeveReembolsar100PorCentoSeCanceladoEm24hAposPagamento()
    {
        var paidAt = _now.AddHours(-10);

        var result = CancellationService.CalculateRefund(BookingClass.Economy, _totalAmount, _now.AddDays(1), paidAt, _now);

        Assert.Equal(1.0m, result.Percentage);
        Assert.Equal(1000m, result.Amount);
    }

    [Fact]
    public void NaoDeveAplicarRegraEspecialAposVinteEQuatroHoras()
    {
        var paidAt = _now.AddHours(-30);

        var result = CancellationService.CalculateRefund(BookingClass.Economy, _totalAmount, _now.AddDays(1), paidAt, _now);

        Assert.Equal(0.0m, result.Percentage);
    }

    [Fact]
    public void DeveFalharComTotalAmountInvalido()
    {
        Assert.Throws<ArgumentException>(() => CancellationService.CalculateRefund(BookingClass.Economy, 0m, _now.AddDays(1), null, _now));
    }
}
