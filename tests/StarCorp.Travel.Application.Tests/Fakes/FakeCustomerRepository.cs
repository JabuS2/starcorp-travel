using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Domain.Customers;

namespace StarCorp.Travel.Application.Tests.Fakes;

public class FakeCustomerRepository : ICustomerRepository
{
    public readonly HashSet<Guid> ExistingCustomers = new();
    public readonly Dictionary<Guid, Customer> Customers = new();

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(ExistingCustomers.Contains(id) || Customers.ContainsKey(id));

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Customers.TryGetValue(id, out var customer);
        return Task.FromResult(customer);
    }
}
