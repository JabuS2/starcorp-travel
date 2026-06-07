namespace StarCorp.Travel.Infrastructure.Persistence;
using System.Data.Common;

public interface ISqlConnectionFactory
{
    DbConnection Create();
}
