using ItemsAdministration.Domain.Dtos.Interfaces;

namespace ItemsAdministration.Domain.Dtos;

public record CreateItemDto(string Code, string Name, string Color, string? Annotations) : IPersistItemDto;