namespace StarCorp.Travel.Domain.Shared;

public static class Guard
{
    public static void ValidateString(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{fieldName} é obrigatório", fieldName);
    }

    public static void ValidateEmail(string email, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"{fieldName} é obrigatório", fieldName);

        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex))
            throw new ArgumentException($"{fieldName} deve ser um email válido", fieldName);
    }

    public static void ValidateCpf(string cpf, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException($"{fieldName} é obrigatório", fieldName);

        cpf = cpf.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11)
            throw new ArgumentException($"{fieldName} deve ter 11 dígitos", fieldName);

        if (new HashSet<char>(cpf).Count == 1)
            throw new ArgumentException($"{fieldName} é inválido", fieldName);

        var sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        }

        var firstDigit = (sum * 10) % 11;
        if (firstDigit == 10) firstDigit = 0;

        if (firstDigit != int.Parse(cpf[9].ToString()))
            throw new ArgumentException($"{fieldName} é inválido", fieldName);

        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        }

        var secondDigit = (sum * 10) % 11;
        if (secondDigit == 10) secondDigit = 0;

        if (secondDigit != int.Parse(cpf[10].ToString()))
            throw new ArgumentException($"{fieldName} é inválido", fieldName);
    }

    public static void ValidateCnpj(string? cnpj, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException($"{fieldName} é obrigatório", fieldName);

        cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

        if (cnpj.Length != 14)
            throw new ArgumentException($"{fieldName} deve ter 14 dígitos", fieldName);

        if (!cnpj.All(char.IsDigit))
            throw new ArgumentException($"{fieldName} é inválido", fieldName);

        if (new HashSet<char>(cnpj).Count == 1)
            throw new ArgumentException($"{fieldName} é inválido", fieldName);

        var sum = 0;
        var weights1 = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        for (int i = 0; i < 12; i++)
            sum += int.Parse(cnpj[i].ToString()) * weights1[i];

        var firstDigit = sum % 11 < 2 ? 0 : 11 - (sum % 11);

        if (firstDigit != int.Parse(cnpj[12].ToString()))
            throw new ArgumentException($"{fieldName} é inválido", fieldName);

        sum = 0;
        var weights2 = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        for (int i = 0; i < 13; i++)
            sum += int.Parse(cnpj[i].ToString()) * weights2[i];

        var secondDigit = sum % 11 < 2 ? 0 : 11 - (sum % 11);

        if (secondDigit != int.Parse(cnpj[13].ToString()))
            throw new ArgumentException($"{fieldName} é inválido", fieldName);
    }
}