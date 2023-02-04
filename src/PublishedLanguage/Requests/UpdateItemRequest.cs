using System;

namespace ItemsAdministration.PublishedLanguage.Requests;

public class UpdateItemRequest
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string? Annotations { get; set; }
}