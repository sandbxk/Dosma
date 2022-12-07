using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    /// <summary>
    ///     Authenticates a user and creates a JWT token.
    ///  
    ///     <para>
    ///         If the user is not found or the password is incorrect, the method returns false.
    ///     </para>
    /// </summary>
    /// <param name="request">The login request.</param>
    /// <param name="token">The JWT token</param>
    /// <returns>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>True</term>
    ///             <description>If the user was authenticated.</description>
    ///         </item>
    ///         <item>
    ///             <term>False</term>
    ///             <description>wrong credentials</description>
    ///         </item>
    ///     </list>
    /// </returns>
    /// <author>
    ///     <name>Mads Mandahl-Barth</name>
    /// </author>
    ///
    public bool Login(LoginRequest request, out string token);

    /// <summary>
    ///     Registers a new user and creates a JWT token.
    ///
    ///     <para>
    ///         If the user already exists, the method returns false.
    ///     </para>
    /// </summary>
    /// <param name="request">The registration request.</param>
    /// <param name="token">The JWT token</param>
    /// <returns>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>True</term>
    ///             <description>If the user was created</description>
    ///         </item>
    ///         <item>
    ///             <term>False</term>
    ///             <description>some error - the user could not be created.</description>
    ///         </item>
    ///     </list>
    /// </returns>
    /// <author>
    ///     <name>Mads Mandahl-Barth</name>
    /// </author>
    public bool Register(RegisterRequest request, out string token);

    public bool AuthenticateToken(string token);

    public User? GetUserFromToken(string token);
}
