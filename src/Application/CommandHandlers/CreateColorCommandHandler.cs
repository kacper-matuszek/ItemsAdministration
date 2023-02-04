using System.Threading;
using System.Threading.Tasks;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.Application.Abstractions.Interfaces.Repositories;
using ItemsAdministration.Application.Consts;
using ItemsAdministration.Common.Application.Abstractions.Exceptions;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Commands;
using ItemsAdministration.Domain.Models;
using MediatR;

namespace ItemsAdministration.Application.CommandHandlers;

internal sealed class CreateColorCommandHandler : ICommandHandler<CreateColorCommand>
{
    private readonly IColorRepository _repository;

    public CreateColorCommandHandler(IColorRepository repository) => 
        _repository = repository;

    public async Task<Unit> Handle(CreateColorCommand command, CancellationToken _)
    {
        var isExistColorWithTheSameName = await _repository.Any(c => c.Name == command.Name);
        if (isExistColorWithTheSameName)
            throw new ObjectAlreadyExistException(Names.Color, new { ColorName = command.Name });

        var color = new Color(command.Name);
        await _repository.Add(color);

        return Unit.Value;
    }
}