namespace StarCorp.Travel.Infrastructure.Repositories;
using Dapper;
using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Application.Common;
using StarCorp.Travel.Domain.Flights;
using StarCorp.Travel.Domain.Shared;
using StarCorp.Travel.Infrastructure.Persistence;

public sealed class FlightRepository : IFlightRepository
{
    private const string Columns = "Id, Origin, Destination, DepartureTime, ArrivalTime, BasePrice, EconomySeats, BusinessSeats, AirlineId, IsActive, CreatedAt, UpdatedAt";
    private readonly ISqlConnectionFactory _connectionFactory;

    public FlightRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Flight?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT {Columns} FROM Flights WHERE Id = @Id;";
        await using var connection = _connectionFactory.Create();
        var row = await connection.QuerySingleOrDefaultAsync<FlightRow>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
        return row?.ToDomain();
    }

    public async Task<PagedResult<Flight>> SearchAsync(FlightSearchCriteria criteria, CancellationToken cancellationToken = default)
    {
        var filters = new List<string> { "IsActive = 1" };
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(criteria.Origin))
        {
            filters.Add("Origin = @Origin");
            parameters.Add("Origin", criteria.Origin);
        }

        if (!string.IsNullOrWhiteSpace(criteria.Destination))
        {
            filters.Add("Destination = @Destination");
            parameters.Add("Destination", criteria.Destination);
        }

        if (criteria.Date.HasValue)
        {
            filters.Add("CAST(DepartureTime AS date) = @Date");
            parameters.Add("Date", criteria.Date.Value.Date);
        }

        if (criteria.MinPrice.HasValue)
        {
            filters.Add("BasePrice >= @MinPrice");
            parameters.Add("MinPrice", criteria.MinPrice.Value);
        }

        if (criteria.MaxPrice.HasValue)
        {
            filters.Add("BasePrice <= @MaxPrice");
            parameters.Add("MaxPrice", criteria.MaxPrice.Value);
        }

        if (criteria.BookingClass.HasValue)
        {
            filters.Add(criteria.BookingClass.Value == BookingClass.Business ? "BusinessSeats > 0" : "EconomySeats > 0");
        }

        var whereClause = string.Join(" AND ", filters);
        parameters.Add("Offset", (criteria.Page - 1) * criteria.PageSize);
        parameters.Add("PageSize", criteria.PageSize);

        var countSql = $"SELECT COUNT(*) FROM Flights WHERE {whereClause};";
        var dataSql = $@"SELECT {Columns} FROM Flights
WHERE {whereClause}
ORDER BY DepartureTime
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        await using var connection = _connectionFactory.Create();
        var total = await connection.ExecuteScalarAsync<int>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
        var rows = await connection.QueryAsync<FlightRow>(new CommandDefinition(dataSql, parameters, cancellationToken: cancellationToken));

        var items = rows.Select(r => r.ToDomain()).ToList();
        return new PagedResult<Flight>(items, criteria.Page, criteria.PageSize, total);
    }

    private sealed class FlightRow
    {
        public Guid Id { get; init; }
        public string Origin { get; init; } = string.Empty;
        public string Destination { get; init; } = string.Empty;
        public DateTime DepartureTime { get; init; }
        public DateTime ArrivalTime { get; init; }
        public decimal BasePrice { get; init; }
        public int EconomySeats { get; init; }
        public int BusinessSeats { get; init; }
        public Guid AirlineId { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }

        public Flight ToDomain() => Flight.Restore(Id, Origin, Destination, DepartureTime, ArrivalTime, BasePrice, EconomySeats, BusinessSeats, AirlineId, IsActive, CreatedAt, UpdatedAt);
    }
}
