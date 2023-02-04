using System.Threading;
using System.Threading.Tasks;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.Application.Abstractions.Interfaces.Repositories;
using ItemsAdministration.Application.Consts;
using ItemsAdministration.Common.Application.Abstractions.Exceptions;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Commands;
using MediatR;

namespace ItemsAdministration.Application.CommandHandlers;

internal sealed class UpdateColorCommandHandler : ICommandHandler<UpdateColorCommand>
{
    private readonly IColorRepository _repository;

    public UpdateColorCommandHandler(IColorRepository repository) => 
        _repository = repository;

    public async Task<Unit> Handle(UpdateColorCommand command, CancellationToken _)
    {
        var color = await _repository.Get(command.Id);
        if (color is null)
            throw new ObjectNotFoundException(Names.Color, new { command.Id });

        if (color.Name != command.Name)
        {
            var isExistColorWithTheSameName = await _repository.Any(c => c.Name == command.Name);
            if (isExistColorWithTheSameName)
                throw new ObjectAlreadyExistException(Names.Color, new { ColorName = command.Name });
        }

        color.Update(command.Name);
        await _repository.Update(color);

        return Unit.Value;
    }
}