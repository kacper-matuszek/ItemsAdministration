using System.Data;

namespace ItemsAdministration.Common.Infrastructure.ReadModel.Factories;

public interface IDatabaseConnectionFactory
{
    IDbConnection CreateDbConnection(string connectionString);
}
