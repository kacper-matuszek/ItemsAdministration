namespace ItemsAdministration.Common.Shared.Exceptions;

public record ValidationResultException(string Code, object? MessageParameter);