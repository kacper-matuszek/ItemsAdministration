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

public sealed class CreateItemCommandHandler : ICommandHandler<CreateItemCommand>
{
    private readonly IItemRepository _itemRepository;
    private readonly IColorRepository _colorRepository;

    public CreateItemCommandHandler(IItemRepository itemRepository, IColorRepository colorRepository)
    {
        _itemRepository = itemRepository;
        _colorRepository = colorRepository;
    }

    public async Task<Unit> Handle(CreateItemCommand command, CancellationToken _)
    {
        var isExistItemWithTheSameCode = await _itemRepository.Any(i => i.Code == command.Dto.Code);
        if (isExistItemWithTheSameCode)
            throw new ObjectAlreadyExistException(Names.Item, new { ItemCode = command.Dto.Code });

        var isColorExist = await _colorRepository.Any(c => c.Name == command.Dto.Color);
        if (!isColorExist)
            throw new ObjectNotFoundException(Names.ItemColor, new { ColorName = command.Dto.Color });

        var item = new Item(command.Dto);
        await _itemRepository.Add(item);

        return Unit.Value;
    }
}