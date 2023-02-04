using System;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Application.Abstractions.Queries;

public record GetColorQuery(Guid Id) : IQuery<ColorResponse>;
