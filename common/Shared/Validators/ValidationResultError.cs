namespace ItemsAdministration.Common.Shared.Exceptions;

public record ValidationResultError(string Code, object? MessageParameter);