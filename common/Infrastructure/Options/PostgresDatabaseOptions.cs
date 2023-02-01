namespace ItemsAdministration.Common.Infrastructure.Options;

public class PostgresDatabaseOptions
{
    public const string SectionName = "PostgresDatabase";

    public string ConnectionString { get; set; } = null!;
}