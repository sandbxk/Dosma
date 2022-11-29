
using Application.DTOs;
using Domain;

namespace Application.Helpers;

/// <summary>
///     helper class for generating objects
/// </summary>
public static class ObjectGenerator
{
    /// <summary>
    ///     Generates a user object
    /// </summary>
    /// <param name="request">The request object</param>
    /// <returns>The generated user object</returns>
    /// <remarks>
    ///     This method is used by
    ///     <list>
    ///         <item><see cref="AuthenticationService.Register"/></item>
    ///     </list>
    /// </remarks>
    /// <completionlist cref="(User, RegisterRequest, HashGenerator, AuthenticationService)"/>
    /// <author>
    ///     <name>Mads Mandahl-Barth</name>
    /// </author>
    public static User GenerateUser(RegisterRequest registration)
    {
        (var salt, var hash) = HashGenerator.Generate(registration.Password);

        return new User
        {
            Username = registration.Username,
            DisplayName = registration.DisplayName,
            HashedPassword = hash,
            Salt = salt
        };
    }
}
