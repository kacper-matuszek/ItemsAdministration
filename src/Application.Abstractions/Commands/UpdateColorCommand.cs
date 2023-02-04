using System;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Commands;

namespace ItemsAdministration.Application.Abstractions.Commands;

public record UpdateColorCommand(Guid Id, string Name) : ICommand;