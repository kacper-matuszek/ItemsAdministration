using System;

namespace ItemsAdministration.Common.Domain.Models.Interfaces;

internal interface IAuditable
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
}