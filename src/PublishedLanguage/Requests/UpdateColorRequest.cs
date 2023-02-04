using System;

namespace ItemsAdministration.PublishedLanguage.Requests;

public class UpdateColorRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}