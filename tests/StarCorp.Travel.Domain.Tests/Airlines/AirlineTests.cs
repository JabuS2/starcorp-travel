using StarCorp.Travel.Domain.Airlines;

namespace StarCorp.Travel.Domain.Tests.Airlines;

public class AirlineTests
{
    [Fact]
    public void DeveCriarCompanhiaAereaComDadosValidos()
    {
        // Arrange
        var name = "Star Airlines";
        var code = "SA";
        var cnpj = "11.222.333/0001-81";

        // Act
        var airline = new Airline(name, code, cnpj);

        // Assert
        Assert.Equal(name, airline.Name);
        Assert.Equal(code, airline.Code);
        Assert.Equal(cnpj, airline.Cnpj);
        Assert.True(airline.IsActive);
        Assert.NotEqual(Guid.Empty, airline.Id);
    }

    [Fact]
    public void NãoDeveCriarCompanhiaAereaComNomeVazio()
    {
        // Arrange
        var name = "";
        var code = "SA";
        var cnpj = "12.345.678/0001-00";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Airline(name, code, cnpj));
    }

    [Fact]
    public void NãoDeveCriarCompanhiaAereaComCodigoVazio()
    {
        // Arrange
        var name = "Star Airlines";
        var code = "";
        var cnpj = "12.345.678/0001-00";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Airline(name, code, cnpj));
    }

    [Fact]
    public void NãoDeveCriarCompanhiaAereaComCnpjVazio()
    {
        // Arrange
        var name = "Star Airlines";
        var code = "SA";
        var cnpj = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Airline(name, code, cnpj));
    }

    [Fact]
    public void DeveCriarCompanhiaAereaAtiva()
    {
        // Arrange
        var name = "Star Airlines";
        var code = "SA";
        var cnpj = "11.222.333/0001-81";

        
        // Act
        var airline = new Airline(name, code, cnpj);
        
        // Assert
        Assert.Equal(name, airline.Name);
        Assert.Equal(code, airline.Code);
        Assert.Equal(cnpj, airline.Cnpj);
        Assert.True(airline.IsActive);
        Assert.NotEqual(Guid.Empty, airline.Id);
    }

    [Fact]
    public void NãoDeveCriarCompanhiaAereaComNomeNulo()
    {
        // Arrange
        string? name = null;
        var code = "SA";
        var cnpj = "12.345.678/0001-00";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Airline(name, code, cnpj));
    }
    
    [Fact]
    public void NãoDeveCriarCompanhiaAereaComCodigoNulo()
    {
        // Arrange
        var name = "Star Airlines";
        string? code = null;
        var cnpj = "12.345.678/0001-00";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Airline(name, code, cnpj));
    }

    [Fact]
    public void NãoDeveCriarCompanhiaAereaComCnpjNulo()
    {
        // Arrange
        var name = "Star Airlines";
        var code = "SA";
        string? cnpj = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Airline(name, code, cnpj));
    }
    [Fact]
    public void NãoDeveCriarCompanhiaAereaComCnpjInvalido()
    {
        // Arrange
        var name = "Star Airlines";
        var code = "SA";
        var cnpj = "12345678000100"; // CNPJ sem formatação

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Airline(name, code, cnpj));
    }
    [Fact]
    public void NãoDeveCriarCompanhiaAereaComCnpjComFormatoInvalido()
    {
        // Arrange
        var name = "Star Airlines";
        var code = "SA";
        var cnpj = "12.345.678/0001-0A"; // CNPJ com caractere inválido

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Airline(name, code, cnpj));
    }
    [Fact]
    public void NãoDeveCriarCompanhiaAereaComCnpjComTamanhoInvalido()
    {
        // Arrange
        var name = "Star Airlines";
        var code = "SA";
        var cnpj = "12.345.678/0001"; // CNPJ com tamanho inválido

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Airline(name, code, cnpj));
    }
    
}