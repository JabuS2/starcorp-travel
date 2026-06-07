namespace StarCorp.Travel.Infrastructure.Persistence;
using System.Data.Common;
using Microsoft.Data.SqlClient;

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection Create() => new SqlConnection(_connectionString);
}
