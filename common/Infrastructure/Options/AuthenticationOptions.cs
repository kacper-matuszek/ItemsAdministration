namespace ItemsAdministration.Common.Infrastructure.Options;

public class AuthenticationOptions
{
    public const string SectionName = "Authentication";

    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
}