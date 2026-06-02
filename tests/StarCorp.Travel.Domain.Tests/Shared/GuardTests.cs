
using StarCorp.Travel.Domain.Shared;
using Xunit;
namespace StarCorp.Travel.Domain.Tests.Shared;

public class GuardTests 
{
    [Fact]
    public void DeveLancarExcecaoQuandoStringForVazia()
    {
        // Arrange
        var value = "";
        var fieldName = "CampoTeste";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateString(value, fieldName));
        Assert.Contains($"{fieldName} é obrigatório", exception.Message);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoStringContemApenasEspacos()
    {
        // Arrange
        var value = "   ";
        var fieldName = "CampoTeste";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateString(value, fieldName));
        Assert.Contains($"{fieldName} é obrigatório", exception.Message);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoStringForNull()
    {
        // Arrange
        string? value = null;
        var fieldName = "CampoTeste";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateString(value, fieldName));
        Assert.Contains($"{fieldName} é obrigatório", exception.Message);
    }


    [Fact]
    public void NãoDeveLancarExcecaoQuandoStringForValida()
    {
        // Arrange
        var value = "Valor Válido";
        var fieldName = "CampoTeste";

        // Act & Assert
        Guard.ValidateString(value, fieldName);
    }

    [Fact]
    public void DeveAceitarEmailValido()
    {
        // Arrange
        var value = "email@dominio.com";
        var fieldName = "Email";

        // Act & Assert
        Guard.ValidateEmail(value, fieldName);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoEmailForInvalido()
    {
        // Arrange
        var value = "email-invalido";
        var fieldName = "Email";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateEmail(value, fieldName));
        Assert.Contains($"{fieldName} deve ser um email válido", exception.Message);
    }

    [Fact]
    public void DeevelancarExcecaoQuandoEmailSemDominio()
    {
        // Arrange
        var value = "email@";
        var fieldName = "Email";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateEmail(value, fieldName));
        Assert.Contains($"{fieldName} deve ser um email válido", exception.Message);
    }
    [Fact]
    public void DeveLancarExcecaoQuandoEmailSemArroba() 
    {
        // Arrange
        var value = "emaildominio.com";
        var fieldName = "Email";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateEmail(value, fieldName));
        Assert.Contains($"{fieldName} deve ser um email válido", exception.Message);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoEmailContemEspacos()
    {
        // Arrange
        var value = "email @dominio.com";
        var fieldName = "Email";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateEmail(value, fieldName));
        Assert.Contains($"{fieldName} deve ser um email válido", exception.Message);
    }
    
    [Fact]
    public void DeveAceitarCpfValido()
    {
        // Arrange
        var value = "472.993.038-09";
        var fieldName = "CPF";

        // Act & Assert
        Guard.ValidateCpf(value, fieldName);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoCpfComMenosDe11Digitos()
    {
        // Arrange
        var value = "123.456.789-0";
        var fieldName = "CPF";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateCpf(value, fieldName));
        Assert.Contains($"{fieldName} deve ter 11 dígitos", exception.Message);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoCpfComMaisDe11Digitos()
    {
        // Arrange
        var value = "123.456.789-000";
        var fieldName = "CPF";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateCpf(value, fieldName));
        Assert.Contains($"{fieldName} deve ter 11 dígitos", exception.Message);
    }
    [Fact]
    public void DeveLancarExcecaoCpfComTodosDigitosIguais()
    {
        // Arrange
        var value = "111.111.111-11";
        var fieldName = "CPF";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateCpf(value, fieldName));
        Assert.Contains($"{fieldName} é inválido", exception.Message);
    }
    [Fact]
    public void DeveLancarExcecaoQuandoCpfComDigitosVerificadosInvalidos()
    {
        // Arrange
        var value = "123.456.789-01";
        var fieldName = "CPF";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateCpf(value, fieldName));
        Assert.Contains($"{fieldName} é inválido", exception.Message);
    }

}