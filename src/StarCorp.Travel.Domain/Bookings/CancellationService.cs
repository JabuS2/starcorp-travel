namespace StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

public record RefundResult(decimal Percentage, decimal Amount);

public static class CancellationService
{
    public static RefundResult CalculateRefund(BookingClass bookingClass, decimal totalAmount, DateTime departureTime, DateTime? paidAt, DateTime now)
    {
        Guard.ValidatePositiveDecimal(totalAmount, nameof(totalAmount));

        if (paidAt.HasValue)
        {
            var sincePaid = now - paidAt.Value;
            if (sincePaid >= TimeSpan.Zero && sincePaid <= TimeSpan.FromHours(24))
                return BuildResult(1.0m, totalAmount);
        }

        var daysUntilDeparture = (departureTime - now).TotalDays;

        decimal percentage;
        if (daysUntilDeparture > 7)
            percentage = 1.0m;
        else if (daysUntilDeparture >= 2)
            percentage = bookingClass == BookingClass.Business ? 0.75m : 0.50m;
        else
            percentage = bookingClass == BookingClass.Business ? 0.25m : 0.0m;

        return BuildResult(percentage, totalAmount);
    }

    private static RefundResult BuildResult(decimal percentage, decimal totalAmount)
        => new(percentage, Math.Round(totalAmount * percentage, 2, MidpointRounding.AwayFromZero));
}
