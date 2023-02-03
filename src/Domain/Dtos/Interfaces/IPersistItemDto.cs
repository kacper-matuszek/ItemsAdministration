namespace ItemsAdministration.Domain.Dtos.Interfaces;

public interface IPersistItemDto
{
    string Code { get; }
    string Name { get; }
    string Color { get; }
    string? Annotations { get; }
}