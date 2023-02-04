using System.Data;
using Npgsql;

namespace ItemsAdministration.Common.Infrastructure.ReadModel.Factories;

public class SqlFactory : ISqlFactory
{
    public IDbConnection CreateDbConnection(string connectionString)
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        return conn;
    }
}