namespace ItemsAdministration.Common.Domain.Models.Interfaces;

public interface IIdentifiable<out T>
    where T : notnull
{
    T Id { get; }
}