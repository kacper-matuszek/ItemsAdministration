using System.Threading;
using System.Threading.Tasks;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.Application.Abstractions.Interfaces.Repositories;
using ItemsAdministration.Application.Consts;
using ItemsAdministration.Common.Application.Abstractions.Exceptions;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Commands;
using MediatR;

namespace ItemsAdministration.Application.CommandHandlers;

public sealed class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand>
{
    private readonly IItemRepository _itemRepository;
    private readonly IColorRepository _colorRepository;

    public UpdateItemCommandHandler(IItemRepository itemRepository, IColorRepository colorRepository)
    {
        _itemRepository = itemRepository;
        _colorRepository = colorRepository;
    }

    public async Task<Unit> Handle(UpdateItemCommand command, CancellationToken _)
    {
        var item = await _itemRepository.Get(command.Id);
        if (item is null)
            throw new ObjectNotFoundException(Names.Item, new { command.Id });

        if (item.Color != command.Dto.Color)
        {
            var isColorExist = await _colorRepository.Any(c => c.Name == command.Dto.Color);
            if (isColorExist)
                throw new ObjectNotFoundException(Names.ItemColor, new { ColorName = command.Dto.Color });
        }

        item.Update(command.Dto);
        await _itemRepository.Update(item);

        return Unit.Value;
    }
}