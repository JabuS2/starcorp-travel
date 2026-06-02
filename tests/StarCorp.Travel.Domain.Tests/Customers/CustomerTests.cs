using StarCorp.Travel.Domain.Customers;

namespace StarCorp.Travel.Domain.Tests.Customers;

public class CustomerTests
{
    [Fact]
    public void DeveCriarClienteComDadosValidos()
    {
        // Arrange
        var name = "João Silva";
        var email = "joao@email.com";
        var phone = "11999999999";
        var address = "Rua das Flores, 123";
        var document = "123.456.789-00";

        // Act
        var customer = new Customer(name, email, phone, address, document);

        // Assert
        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
        Assert.True(customer.IsActive);
        Assert.NotEqual(Guid.Empty, customer.Id);
    }

    [Fact]
    public void NãoDeveCriarClienteComNomeVazio()
    {
        // Arrange
        var name = "";
        var email = "joao@email.com";
        var phone = "11999999999";
        var address = "Rua das Flores, 123";
        var document = "123.456.789-00";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(name, email, phone, address, document));
    }

    [Fact]
    public void NãoDeveCriarClienteComEmailVazio()
    {
        // Arrange
        var name = "João Silva";
        var email = "";
        var phone = "11999999999";
        var address = "Rua das Flores, 123";
        var document = "123.456.789-00";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(name, email, phone, address, document));
    }   

    [Fact]
    public void NãoDeveCriarClienteComTelefoneVazio()
    {
        // Arrange
        var name = "João Silva";
        var email = "joao@email.com";
        var phone = "";
        var address = "Rua das Flores, 123";
        var document = "123.456.789-00";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(name, email, phone, address, document));
    }

    [Fact]
    public void NãoDeveCriarClienteComEnderecoVazio()
    {
        // Arrange
        var name = "João Silva";
        var email = "joao@email.com";
        var phone = "11999999999";
        var address = "";
        var document = "123.456.789-00";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(name, email, phone, address, document));
    }

    [Fact]
    public void NãoDeveCriarClienteComDocumentoVazio()
    {
        // Arrange
        var name = "João Silva";
        var email = "joao@email.com";
        var phone = "11999999999";
        var address = "Rua das Flores, 123";
        var document = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(name, email, phone, address, document));
    }

    [Fact]
    public void DeveCriarClienteAtivo()
    {
        // Arrange
        var name = "João Silva";
        var email = "joao@email.com";
        var phone = "11999999999";
        var address = "Rua das Flores, 123";
        var document = "123.456.789-00";

        // Act
        var customer = new Customer(name, email, phone, address, document);

        // Assert
        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
        Assert.True(customer.IsActive);
        Assert.NotEqual(Guid.Empty, customer.Id);
    }   
}