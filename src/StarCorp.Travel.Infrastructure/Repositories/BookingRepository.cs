namespace StarCorp.Travel.Infrastructure.Repositories;
using Dapper;
using StarCorp.Travel.Application.Abstractions;
using StarCorp.Travel.Domain.Bookings;
using StarCorp.Travel.Domain.Shared;
using StarCorp.Travel.Infrastructure.Persistence;

public sealed class BookingRepository : IBookingRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public BookingRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        const string insertBooking = @"INSERT INTO Bookings (Id, CustomerId, FlightId, BookingClass, Status, TotalAmount, CreatedAt, UpdatedAt)
VALUES (@Id, @CustomerId, @FlightId, @BookingClass, @Status, @TotalAmount, @CreatedAt, @UpdatedAt);";

        const string insertPassenger = @"INSERT INTO Passengers (Id, BookingId, Name, Document, CreatedAt, UpdatedAt)
VALUES (@Id, @BookingId, @Name, @Document, @CreatedAt, @UpdatedAt);";

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        await connection.ExecuteAsync(new CommandDefinition(insertBooking, new
        {
            booking.Id,
            booking.CustomerId,
            booking.FlightId,
            BookingClass = (int)booking.BookingClass,
            Status = (int)booking.Status,
            booking.TotalAmount,
            booking.CreatedAt,
            booking.UpdatedAt
        }, transaction, cancellationToken: cancellationToken));

        foreach (var passenger in booking.Passengers)
        {
            await connection.ExecuteAsync(new CommandDefinition(insertPassenger, new
            {
                passenger.Id,
                BookingId = booking.Id,
                passenger.Name,
                passenger.Document,
                passenger.CreatedAt,
                passenger.UpdatedAt
            }, transaction, cancellationToken: cancellationToken));
        }

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string bookingSql = @"SELECT Id, CustomerId, FlightId, BookingClass, Status, TotalAmount, CreatedAt, UpdatedAt
FROM Bookings WHERE Id = @Id;";
        const string passengersSql = @"SELECT Id, Name, Document, CreatedAt, UpdatedAt
FROM Passengers WHERE BookingId = @Id;";

        await using var connection = _connectionFactory.Create();
        var bookingRow = await connection.QuerySingleOrDefaultAsync<BookingRow>(new CommandDefinition(bookingSql, new { Id = id }, cancellationToken: cancellationToken));
        if (bookingRow is null)
            return null;

        var passengerRows = await connection.QueryAsync<PassengerRow>(new CommandDefinition(passengersSql, new { Id = id }, cancellationToken: cancellationToken));
        var passengers = passengerRows.Select(p => p.ToDomain()).ToList();

        return bookingRow.ToDomain(passengers);
    }

    public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        const string sql = @"UPDATE Bookings
SET Status = @Status, TotalAmount = @TotalAmount, UpdatedAt = @UpdatedAt
WHERE Id = @Id;";

        await using var connection = _connectionFactory.Create();
        await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            booking.Id,
            Status = (int)booking.Status,
            booking.TotalAmount,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken: cancellationToken));
    }

    public async Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        const string sql = @"INSERT INTO Payments (Id, BookingId, CustomerId, Amount, PaymentMethod, Status, PaidAt, CreatedAt, UpdatedAt)
VALUES (@Id, @BookingId, @CustomerId, @Amount, @PaymentMethod, @Status, @PaidAt, @CreatedAt, @UpdatedAt);";

        await using var connection = _connectionFactory.Create();
        await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            payment.Id,
            payment.BookingId,
            payment.CustomerId,
            payment.Amount,
            PaymentMethod = (int)payment.PaymentMethod,
            Status = (int)payment.Status,
            payment.PaidAt,
            payment.CreatedAt,
            payment.UpdatedAt
        }, cancellationToken: cancellationToken));
    }

    public async Task<Payment?> GetPaymentByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT TOP 1 Id, BookingId, CustomerId, Amount, PaymentMethod, Status, PaidAt, CreatedAt, UpdatedAt
FROM Payments WHERE BookingId = @BookingId ORDER BY CreatedAt DESC;";

        await using var connection = _connectionFactory.Create();
        var row = await connection.QuerySingleOrDefaultAsync<PaymentRow>(new CommandDefinition(sql, new { BookingId = bookingId }, cancellationToken: cancellationToken));
        return row?.ToDomain();
    }

    private sealed class BookingRow
    {
        public Guid Id { get; init; }
        public Guid CustomerId { get; init; }
        public Guid FlightId { get; init; }
        public int BookingClass { get; init; }
        public int Status { get; init; }
        public decimal TotalAmount { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }

        public Booking ToDomain(IEnumerable<Passenger> passengers)
            => Booking.Restore(Id, CustomerId, FlightId, (BookingClass)BookingClass, (BookingStatus)Status, TotalAmount, passengers, CreatedAt, UpdatedAt);
    }

    private sealed class PassengerRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Document { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }

        public Passenger ToDomain() => Passenger.Restore(Id, Name, Document, CreatedAt, UpdatedAt);
    }

    private sealed class PaymentRow
    {
        public Guid Id { get; init; }
        public Guid BookingId { get; init; }
        public Guid CustomerId { get; init; }
        public decimal Amount { get; init; }
        public int PaymentMethod { get; init; }
        public int Status { get; init; }
        public DateTime? PaidAt { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }

        public Payment ToDomain()
            => Payment.Restore(Id, BookingId, CustomerId, Amount, (PaymentMethod)PaymentMethod, (PaymentStatus)Status, PaidAt, CreatedAt, UpdatedAt);
    }
}
