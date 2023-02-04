using System;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Commands;
using ItemsAdministration.Domain.Dtos;

namespace ItemsAdministration.Application.Abstractions.Commands;

public record UpdateItemCommand(Guid Id, UpdateItemDto Dto) : ICommand;