using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Providers;
using ItemsAdministration.Common.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ItemsAdministration.Common.Infrastructure.Providers;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly AuthenticationOptions _options;

    public JwtTokenProvider(IOptions<AuthenticationOptions> options) =>
        _options = options.Value;

    public string Generate(Guid userId, string[] roleNames)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
        };
        claims.AddRange(GenerateClaimRoles(roleNames));

        var expiration = DateTime.Now.AddDays(1);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            signingCredentials: credentials,
            claims: claims,
            expires: expiration);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static Claim[] GenerateClaimRoles(string[] roles) => 
        roles.Select(role => new Claim(ClaimTypes.Role, role)).ToArray();
}