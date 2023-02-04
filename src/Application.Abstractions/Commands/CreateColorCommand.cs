using ItemsAdministration.Common.Application.Abstractions.Interfaces.Commands;

namespace ItemsAdministration.Application.Abstractions.Commands;

public record CreateColorCommand(string Name) : ICommand;