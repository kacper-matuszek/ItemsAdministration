using System;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Application.Abstractions.Queries;

public record GetItemQuery(Guid Id) : IQuery<ItemResponse>;