using System;
using ItemsAdministration.Common.Domain.Models;
using ItemsAdministration.Domain.Validators;

namespace ItemsAdministration.Domain.Models;

//I created as aggregate (not value object), because in technical design author said: "it can be modified by another 'things' and should be stored as table (not as a child)"
public class Color : BaseGuidAggregate
{
    public Color(string name)
        : base(Guid.NewGuid())
    {
        Name = name;
        new ColorValidator().ValidateAndThrow(this);
    }

    private Color()
        : base(default)
    { }

    public string Name { get; private set; } = null!;

    public void Update(string name)
    {
        Name = name;
        new ColorValidator().ValidateAndThrow(this);
    }
}