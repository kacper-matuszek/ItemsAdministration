using System.Data;

namespace ItemsAdministration.Common.Infrastructure.ReadModel.Factories;

public interface ISqlFactory
{
    IDbConnection CreateDbConnection(string connectionString);
}