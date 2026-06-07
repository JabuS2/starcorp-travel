namespace StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;

public class Passenger : Entity
{
    public string Name { get; private set; }
    public string Document { get; private set; }

    public Passenger(string name, string document)
    {
        Guard.ValidateString(name, nameof(name));
        Guard.ValidateString(document, nameof(document));
        Guard.ValidateCpf(document, nameof(document));

        Name = name;
        Document = document;
    }
}