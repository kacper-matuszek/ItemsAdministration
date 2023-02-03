namespace ItemsAdministration.Common.Shared.Exceptions;

public record LocalizedError(string Code, object? MessageParameter);