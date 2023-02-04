namespace ItemsAdministration.PublishedLanguage.Requests;

public class CreateItemRequest
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string? Annotations { get; set; }
}