namespace StarCorp.Travel.Domain.Flights;
using StarCorp.Travel.Domain.Shared;

public class Flight : Entity
{
    public string Origin { get; private set; }
    public string Destination { get; private set; }
    public DateTime DepartureTime { get; private set; }
    public DateTime ArrivalTime { get; private set; }
    public decimal BasePrice { get; private set; }
    public int EconomySeats { get; private set; }
    public int BusinessSeats { get; private set; }
    public Guid AirlineId { get; private set; }
    public bool IsActive { get; private set; }

    public Flight(string origin, string destination, DateTime departureTime, DateTime arrivalTime, decimal basePrice, int economySeats, int businessSeats, Guid airlineId)
    {
        Guard.ValidateString(origin, nameof(origin));
        Guard.ValidateString(destination, nameof(destination));
        Guard.ValidatePositiveDecimal(basePrice, nameof(basePrice));
        Guard.ValidatePositiveInteger(economySeats, nameof(economySeats));
        Guard.ValidatePositiveInteger(businessSeats, nameof(businessSeats));
        Guard.ValidateArrivalAfterDeparture(arrivalTime, departureTime, nameof(arrivalTime));

        Origin = origin;
        Destination = destination;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        BasePrice = basePrice;
        EconomySeats = economySeats;
        BusinessSeats = businessSeats;
        AirlineId = airlineId;
        IsActive = true;
    }

    public static Flight Restore(Guid id, string origin, string destination, DateTime departureTime, DateTime arrivalTime, decimal basePrice, int economySeats, int businessSeats, Guid airlineId, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        var flight = new Flight(origin, destination, departureTime, arrivalTime, basePrice, economySeats, businessSeats, airlineId);
        flight.SetPersistenceData(id, createdAt, updatedAt);
        flight.IsActive = isActive;
        return flight;
    }
}