using System.Data;
using Npgsql;

namespace ItemsAdministration.Common.Infrastructure.ReadModel.Factories;

public partial class SqlFactory : IDatabaseConnectionFactory
{
    public IDbConnection CreateDbConnection(string connectionString)
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        return conn;
    }
}
