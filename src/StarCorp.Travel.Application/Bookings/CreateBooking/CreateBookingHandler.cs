namespace StarCorp.Travel.Application.Bookings.CreateBooking;
using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Application.Common.Exceptions;
using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

public class CreateBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly ICustomerRepository _customerRepository;

    public CreateBookingHandler(
        IBookingRepository bookingRepository,
        IFlightRepository flightRepository,
        ICustomerRepository customerRepository)
    {
        _bookingRepository = bookingRepository;
        _flightRepository = flightRepository;
        _customerRepository = customerRepository;
    }

    public async Task<CreateBookingResponse> HandleAsync(CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Passengers is null || request.Passengers.Count == 0)
            throw new ArgumentException("A reserva deve conter ao menos um passageiro", nameof(request.Passengers));

        if (!await _customerRepository.ExistsAsync(request.CustomerId, cancellationToken))
            throw new NotFoundException($"Cliente {request.CustomerId} não encontrado");

        var flight = await _flightRepository.GetByIdAsync(request.FlightId, cancellationToken)
            ?? throw new NotFoundException($"Voo {request.FlightId} não encontrado");

        var passengerCount = request.Passengers.Count;
        var availableSeats = request.BookingClass == BookingClass.Business ? flight.BusinessSeats : flight.EconomySeats;
        if (passengerCount > availableSeats)
            throw new ConflictException("Não há assentos suficientes disponíveis para a classe selecionada");

        var breakdown = PricingService.Calculate(flight.BasePrice, passengerCount, request.BookingClass, request.PaymentMethod);

        var passengers = request.Passengers
            .Select(p => new Passenger(p.Name, p.Document))
            .ToList();

        var booking = new Booking(request.CustomerId, request.FlightId, request.BookingClass, breakdown.Total, passengers);

        await _bookingRepository.AddAsync(booking, cancellationToken);

        return new CreateBookingResponse(
            booking.Id,
            booking.CustomerId,
            booking.FlightId,
            booking.BookingClass,
            booking.Status,
            booking.TotalAmount,
            booking.Passengers.Select(p => new PassengerResponse(p.Name, p.Document)).ToList(),
            new PriceBreakdownResponse(breakdown.Subtotal, breakdown.Taxes, breakdown.ServiceFee, breakdown.PaymentAdjustment, breakdown.Total));
    }
}
