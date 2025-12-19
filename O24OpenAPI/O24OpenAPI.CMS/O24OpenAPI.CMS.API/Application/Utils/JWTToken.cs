using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using O24OpenAPI.CMS.API.Application.Models.Request;

namespace O24OpenAPI.CMS.API.Application.Utils;

public static class JWT
{
    public static string GenerateStaticTokenJwt(
        string clientId,
        string secretKey,
        IReadOnlyCollection<StaticTokenScope> scopes,
        DateTime? expiredOnUtc = null,
        string issuer = "O24OpenAPI",
        string audience = "O24OpenAPI"
    )
    {
        var now = DateTimeOffset.UtcNow;
        var expires = expiredOnUtc ?? now.AddDays(7);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, clientId),
            new(
                JwtRegisteredClaimNames.Iat,
                now.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64
            ),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new("scope", string.Join(" ", scopes.Select(s => s.ToString()))),
            new("token_type", "static"),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expires.UtcDateTime,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
