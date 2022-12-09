using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Application.Helpers;

/// <summary>
///     helper class for generating tokens
/// </summary>
public static class TokenGenerator
{
    /// <summary>
    ///     Generates a JWT token
    /// </summary>
    /// <param name="user">The user object</param>
    /// <param name="secret">The secret used to sign the token</param>
    /// <returns>The generated token</returns>
    /// <remarks>
    ///     This method is used by
    ///     <list>
    ///         <item><see cref="AuthenticationService.Login"/></item>
    ///         <item><see cref="AuthenticationService.Register"/></item>
    ///     </list>
    /// </remarks>
    /// <completionlist cref="(User, AuthenticationService)"/>
    /// <author>
    ///     <name>Mads Mandahl-Barth</name>
    /// </author>
    public static string GenerateToken(User user, byte[] secret)
    {
        List<Claim> claims = new()
        {
            new Claim("id", user.Id.ToString()),
            new Claim("name", user.DisplayName ?? user.Username),
            new Claim("username", user.Username),
            new Claim("lists", JsonConvert.SerializeObject(user.GroceryList.Select(x => x.Id)))
        };
        
        var payload = new JwtPayload(null, null, claims, DateTime.Now, DateTime.Now.AddMinutes(45));
        var header = new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha512));
        var token = new JwtSecurityToken(header, payload);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
