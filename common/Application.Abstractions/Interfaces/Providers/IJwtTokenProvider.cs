using System;

namespace ItemsAdministration.Common.Application.Abstractions.Interfaces.Providers;

public interface IJwtTokenProvider
{
    string Generate(Guid userId, string[] roleNames);
}