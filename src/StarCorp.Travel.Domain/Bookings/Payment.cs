namespace StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

public class Payment : Entity
{
    public Guid BookingId { get; private set; }
    public Guid CustomerId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }

    public Payment(Guid bookingId, Guid customerId, decimal amount, PaymentMethod paymentMethod)
    {
        Guard.ValidateGuid(bookingId, nameof(bookingId));
        Guard.ValidateGuid(customerId, nameof(customerId));
        Guard.ValidatePositiveDecimal(amount, nameof(amount));

        BookingId = bookingId;
        CustomerId = customerId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Status = PaymentStatus.Pending;
    }

    public void Confirm(DateTime confirmDate)
    {
        Status = PaymentStatus.Confirmed;
        PaidAt = confirmDate;
    }

    public static Payment Restore(Guid id, Guid bookingId, Guid customerId, decimal amount, PaymentMethod paymentMethod, PaymentStatus status, DateTime? paidAt, DateTime createdAt, DateTime updatedAt)
    {
        var payment = new Payment(bookingId, customerId, amount, paymentMethod);
        payment.SetPersistenceData(id, createdAt, updatedAt);
        payment.Status = status;
        payment.PaidAt = paidAt;
        return payment;
    }
}