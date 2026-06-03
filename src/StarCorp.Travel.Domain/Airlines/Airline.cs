namespace StarCorp.Travel.Domain.Airlines;
using StarCorp.Travel.Domain.Shared;

public class Airline : Entity
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public string Cnpj { get; private set; }
    public bool IsActive { get; private set; }

    public Airline(string? name, string? code, string? cnpj)
    {
        Guard.ValidateString(name, nameof(name));
        Guard.ValidateString(code, nameof(code));
        Guard.ValidateString(cnpj, nameof(cnpj));

        Guard.ValidateCnpj(cnpj, nameof(cnpj));

        Name = name!;
        Code = code!;
        Cnpj = cnpj!;
        IsActive = true;
    }
}
