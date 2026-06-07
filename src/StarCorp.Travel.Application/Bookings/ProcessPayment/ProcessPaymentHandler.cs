namespace StarCorp.Travel.Application.Bookings.ProcessPayment;
using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Application.Common.Exceptions;
using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

public class ProcessPaymentHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ProcessPaymentHandler(IBookingRepository bookingRepository, IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ProcessPaymentResponse> HandleAsync(Guid bookingId, ProcessPaymentRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken)
            ?? throw new NotFoundException($"Reserva {bookingId} não encontrada");

        if (booking.Status == BookingStatus.Cancelled)
            throw new ConflictException("Não é possível pagar uma reserva cancelada");

        var existingPayment = await _bookingRepository.GetPaymentByBookingIdAsync(bookingId, cancellationToken);
        if (existingPayment is not null && existingPayment.Status == PaymentStatus.Confirmed)
            throw new ConflictException("A reserva já foi paga");

        var calculation = PricingService.CalculatePayment(booking.TotalAmount, request.PaymentMethod);

        var payment = new Payment(booking.Id, booking.CustomerId, calculation.Total, request.PaymentMethod);
        payment.Confirm(_dateTimeProvider.UtcNow);

        booking.Settle(calculation.Total);

        await _bookingRepository.AddPaymentAsync(payment, cancellationToken);
        await _bookingRepository.UpdateAsync(booking, cancellationToken);

        return new ProcessPaymentResponse(
            payment.Id,
            payment.BookingId,
            calculation.BaseTotal,
            calculation.PaymentAdjustment,
            payment.Amount,
            payment.PaymentMethod,
            payment.Status,
            payment.PaidAt,
            booking.Status);
    }
}
