using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace Application.Helpers;

public static class TokenGenerator
{
    public static string GenerateToken(User user, byte[] serverSecret)
    {
        List<Claim> claims = new()
        {
            new Claim("id", user.Id.ToString()),
            new Claim("name", user.DisplayName),
            new Claim("username", user.Username),
        };

        var payload = new JwtPayload(null, null, claims, DateTime.Now, DateTime.Now.AddMinutes(45));
        var header = new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(serverSecret), SecurityAlgorithms.HmacSha512));
        var token = new JwtSecurityToken(header, payload);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
