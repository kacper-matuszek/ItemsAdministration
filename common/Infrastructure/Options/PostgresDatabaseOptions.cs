using ItemsAdministration.Common.Infrastructure.ReadModel;

namespace ItemsAdministration.Common.Infrastructure.Options;

public class PostgresDatabaseOptions : IDatabaseOptions
{
    public const string SectionName = "PostgresDatabase";

    public string ConnectionString { get; set; } = null!;
}