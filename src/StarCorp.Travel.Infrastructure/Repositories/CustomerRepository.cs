namespace StarCorp.Travel.Infrastructure.Repositories;
using Dapper;
using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Domain.Customers;
using StarCorp.Travel.Infrastructure.Persistence;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CustomerRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT Id, Name, Email, PhoneNumber, Address, DocumentNumber, IsActive, CreatedAt, UpdatedAt
FROM Customers WHERE Id = @Id;";

        await using var connection = _connectionFactory.Create();
        var row = await connection.QuerySingleOrDefaultAsync<CustomerRow>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
        return row?.ToDomain();
    }

    private sealed class CustomerRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string DocumentNumber { get; init; } = string.Empty;
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }

        public Customer ToDomain()
            => Customer.Restore(Id, Name, Email, PhoneNumber, Address, DocumentNumber, IsActive, CreatedAt, UpdatedAt);
    }
}
