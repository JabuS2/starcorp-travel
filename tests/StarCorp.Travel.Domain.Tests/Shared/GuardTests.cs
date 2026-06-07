
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

    [Fact]
    public void DeveLancarExcecaoQuandoPrecosMenorOuIgualAZero()
    {
        // Arrange
        var value = 0m;
        var fieldName = "Preço";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidatePositiveDecimal(value, fieldName));
        Assert.Contains($"{fieldName} deve ser maior que zero", exception.Message);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoAssentosMenorQueZero()
    {
        // Arrange
        var value = -1;
        var fieldName = "Assentos";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidatePositiveInteger(value, fieldName));
        Assert.Contains($"{fieldName} deve ser um valor positivo", exception.Message);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoDataChegadaAnteriorDataPartida()
    {
        // Arrange
        var departure = DateTime.Now;
        var arrival = departure.AddHours(-1);
        var fieldName = "Data de Chegada";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateArrivalAfterDeparture(arrival, departure, fieldName));
        Assert.Contains($"{fieldName} deve ser posterior à data de partida", exception.Message);
    }

    [Fact]
    public void DeveAceitarPrecosMaioresQueZero()
    {
        // Arrange
        var value = 100m;
        var fieldName = "Preço";

        // Act & Assert
        Guard.ValidatePositiveDecimal(value, fieldName);
    }
    [Fact]
    public void DeveAceitarAssentosMaioresOuIgualAZero()
    {
        // Arrange
        var value = 0;
        var fieldName = "Assentos";

        // Act & Assert
        Guard.ValidatePositiveInteger(value, fieldName);
    }
    [Fact]
    public void DeveAceitarDataChegadaPosteriorDataPartida()
    {
        // Arrange
        var departure = DateTime.Now;
        var arrival = departure.AddHours(1);
        var fieldName = "Data de Chegada";

        // Act & Assert
        Guard.ValidateArrivalAfterDeparture(arrival, departure, fieldName);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoGuidForVazio()
    {
        // Arrange
        var value = Guid.Empty;
        var fieldName = "ID";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Guard.ValidateGuid(value, fieldName));
        Assert.Contains($"{fieldName} é obrigatório", exception.Message);
    }

    [Fact]
    public void DeveAceitarGuidValido()
    {
        // Arrange
        var value = Guid.NewGuid();
        var fieldName = "ID";

        // Act & Assert
        Guard.ValidateGuid(value, fieldName);
    }
}