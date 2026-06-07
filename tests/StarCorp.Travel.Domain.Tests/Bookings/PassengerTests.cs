using StarCorp.Travel.Domain.Bookings;

namespace StarCorp.Travel.Domain.Tests.Bookings;

public class PassengerTests
{
    private readonly string _name = "John Doe";
    private readonly string _document = "47299303809";

   [Fact]
   public void DeveCriarPassageiroComDadosValidos()
   {
       // Act
       var passenger = new Passenger(_name, _document);

       // Assert
       Assert.Equal(_name, passenger.Name);
       Assert.Equal(_document, passenger.Document);
       Assert.NotEqual(Guid.Empty, passenger.Id);
   }

   [Fact]
   public void DeveFalharAoCriarPassageiroComNomeVazio()
   {
       // Act & Assert
       Assert.Throws<ArgumentException>(() => new Passenger(string.Empty, _document));
   }

    [Fact]
    public void DeveFalharAoCriarPassageiroComDocumentVazio()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Passenger(_name, string.Empty));
    }
    [Fact]
    public void DeveFalharAoCriarPassageiroComNomeNulo()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Passenger(null!, _document));
    }
    [Fact]
    public void DeveFalharAoCriarPassageiroComDocumentNulo()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Passenger(_name, null!));
    }
    [Fact]
    public void DeveFalharAoCriarPassageiroComDocumentInvalido()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Passenger(_name, "document-invalido"));
    }
} 