namespace StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

public record PriceBreakdown(decimal Subtotal, decimal Taxes, decimal ServiceFee, decimal PaymentAdjustment, decimal Total);

public static class PricingService
{
    private const decimal EconomyMultiplier = 1.0m;
    private const decimal BusinessMultiplier = 2.5m;
    private const decimal TaxRate = 0.08m;
    private const decimal TaxPerPassenger = 45m;
    private const decimal ServiceFeeRate = 0.05m;
    private const decimal CreditCardAdjustmentRate = 0.03m;
    private const decimal PixAdjustmentRate = -0.05m;
    private const decimal BoletoAdjustmentRate = 0.01m;

    public static PriceBreakdown Calculate(decimal basePrice, int passengerCount, BookingClass bookingClass, PaymentMethod paymentMethod)
    {
        Guard.ValidatePositiveDecimal(basePrice, nameof(basePrice));

        if (passengerCount <= 0)
            throw new ArgumentException("A reserva deve conter ao menos um passageiro", nameof(passengerCount));

        var multiplier = bookingClass == BookingClass.Business ? BusinessMultiplier : EconomyMultiplier;
        var subtotal = basePrice * passengerCount * multiplier;
        var taxes = subtotal * TaxRate + TaxPerPassenger * passengerCount;
        var subtotalWithTaxes = subtotal + taxes;
        var serviceFee = subtotalWithTaxes * ServiceFeeRate;
        var totalBeforeAdjustment = subtotalWithTaxes + serviceFee;

        var adjustmentRate = paymentMethod switch
        {
            PaymentMethod.CreditCard => CreditCardAdjustmentRate,
            PaymentMethod.Pix => PixAdjustmentRate,
            PaymentMethod.Boleto => BoletoAdjustmentRate,
            _ => 0m
        };

        var paymentAdjustment = totalBeforeAdjustment * adjustmentRate;
        var total = totalBeforeAdjustment + paymentAdjustment;

        return new PriceBreakdown(
            Round(subtotal),
            Round(taxes),
            Round(serviceFee),
            Round(paymentAdjustment),
            Round(total));
    }

    private static decimal Round(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
