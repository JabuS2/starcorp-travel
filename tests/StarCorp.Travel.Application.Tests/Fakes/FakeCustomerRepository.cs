using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Domain.Customers;

namespace StarCorp.Travel.Application.Tests.Fakes;

public class FakeCustomerRepository : ICustomerRepository
{
    public readonly Dictionary<Guid, Customer> Customers = new();

    public Guid Add(bool isActive = true)
    {
        var id = Guid.NewGuid();
        Customers[id] = Customer.Restore(id, "Cliente Teste", "cliente@example.com", "+5511999999999", "Rua Teste, 1", "47299303809", isActive, DateTime.UtcNow, DateTime.UtcNow);
        return id;
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Customers.TryGetValue(id, out var customer);
        return Task.FromResult(customer);
    }
}
