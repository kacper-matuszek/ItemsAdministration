using System;
using ItemsAdministration.Common.Domain.Models;
using ItemsAdministration.Domain.Dtos;
using ItemsAdministration.Domain.Dtos.Interfaces;
using ItemsAdministration.Domain.Validators;

namespace ItemsAdministration.Domain.Models;

public class Item : BaseGuidAggregate
{
    public Item(CreateItemDto dto)
        : base(Guid.NewGuid())
    {
        Persist(dto);
        new ItemValidator().ValidateAndThrow(this);
    }

    private Item()
        : base(default)
    { }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Color { get; private set; } = null!;
    public string? Annotations { get; private set; }

    public void Update(UpdateItemDto dto)
    {
        Persist(dto);
    }

    private void Persist(IPersistItemDto dto)
    {
        Code = dto.Code;
        Name = dto.Name;
        Color = dto.Color;
        Annotations = dto.Annotations;
    }
}