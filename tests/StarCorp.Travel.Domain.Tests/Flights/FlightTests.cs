using StarCorp.Travel.Domain.Flights;

namespace StarCorp.Travel.Domain.Tests.Flights;

public class FlightTests
{
    private readonly Guid _airlineId = Guid.NewGuid();
    private readonly string _origin = "São Paulo";
    private readonly string _destination = "Rio de Janeiro";
    private readonly DateTime _departure = DateTime.UtcNow.AddDays(10);
    private readonly DateTime _arrival = DateTime.UtcNow.AddDays(10).AddHours(1);
    private readonly decimal _basePrice = 500m;
    private readonly int _economySeats = 100;
    private readonly int _businessSeats = 20;

    [Fact]
    public void DeveCriarVooComDadosValidos()
    {
        // Act
        var flight = new Flight(_origin, _destination, _departure, _arrival, _basePrice, _economySeats, _businessSeats, _airlineId);

        // Assert
        Assert.Equal(_origin, flight.Origin);
        Assert.Equal(_destination, flight.Destination);
        Assert.Equal(_basePrice, flight.BasePrice);
        Assert.Equal(_economySeats, flight.EconomySeats);
        Assert.Equal(_businessSeats, flight.BusinessSeats);
        Assert.Equal(_airlineId, flight.AirlineId);
        Assert.NotEqual(Guid.Empty, flight.Id);
    }

    [Fact]
    public void DeveFalharAoCriarVooComOrigemInvalida()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Flight(string.Empty, _destination, _departure, _arrival, _basePrice, _economySeats, _businessSeats, _airlineId));
    }

    [Fact]
    public void DeveFalharAoCriarVooComDestinoInvalido()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Flight(_origin, string.Empty, _departure, _arrival, _basePrice, _economySeats, _businessSeats, _airlineId));
    }

    [Fact]
    public void DeveFalharAoCriarVooComAssentosEconomyInvalidos()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Flight(_origin, _destination, _departure, _arrival, _basePrice, -1, _businessSeats, _airlineId));
    }

    [Fact]
    public void DeveFalharAoCriarVooComAssentosBusinessInvalidos()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Flight(_origin, _destination, _departure, _arrival, _basePrice, _economySeats, -1, _airlineId));
    }

    [Fact]
    public void DeveFalharAoCriarDataDeChegadaAnteriorADepartida()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Flight(_origin, _destination, _departure, _departure.AddHours(-1), _basePrice, _economySeats, _businessSeats, _airlineId));
    }

    [Fact]
    public void DevefalharAoCriarPrecoMenorOuIgualAZero()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Flight(_origin, _destination, _departure, _arrival, 0m, _economySeats, _businessSeats, _airlineId));
    }

}
