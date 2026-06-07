namespace StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

public class Booking : Entity
{
    private readonly List<Passenger> _passengers = new();

    public Guid CustomerId { get; private set; }
    public Guid FlightId { get; private set; }
    public BookingClass BookingClass { get; private set; }
    public BookingStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public IReadOnlyList<Passenger> Passengers => _passengers.AsReadOnly();

    public Booking(Guid customerId, Guid flightId, BookingClass bookingClass, decimal totalAmount, IEnumerable<Passenger> passengers)
    {
        Guard.ValidateGuid(customerId, nameof(customerId));
        Guard.ValidateGuid(flightId, nameof(flightId));
        Guard.ValidatePositiveDecimal(totalAmount, nameof(totalAmount));

        var passengerList = passengers?.ToList() ?? new List<Passenger>();
        if (passengerList.Count == 0)
            throw new ArgumentException("A reserva deve conter ao menos um passageiro", nameof(passengers));

        CustomerId = customerId;
        FlightId = flightId;
        BookingClass = bookingClass;
        TotalAmount = totalAmount;
        Status = BookingStatus.Pending;
        _passengers.AddRange(passengerList);
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new InvalidOperationException("Apenas reservas pendentes podem ser confirmadas");

        Status = BookingStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("A reserva já está cancelada");

        Status = BookingStatus.Cancelled;
    }

    public static Booking Restore(Guid id, Guid customerId, Guid flightId, BookingClass bookingClass, BookingStatus status, decimal totalAmount, IEnumerable<Passenger> passengers, DateTime createdAt, DateTime updatedAt)
    {
        var booking = new Booking(customerId, flightId, bookingClass, totalAmount, passengers);
        booking.SetPersistenceData(id, createdAt, updatedAt);
        booking.Status = status;
        return booking;
    }
}
