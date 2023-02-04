using System;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Providers;
using ItemsAdministration.Infrastructure.Api.Consts;
using Microsoft.AspNetCore.Mvc;

namespace ItemsAdministration.Infrastructure.Api.Controllers;

//Only for testing roles
[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public AuthController(IJwtTokenProvider jwtTokenProvider) => 
        _jwtTokenProvider = jwtTokenProvider;

    [HttpPost]
    public string GenerateToken([FromQuery] bool applyRole) => 
        _jwtTokenProvider.Generate(Guid.NewGuid(), applyRole ? new []{ RoleNames.ItemsManagement } : Array.Empty<string>());
}