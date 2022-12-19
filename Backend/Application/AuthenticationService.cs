using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;
using Application.Helpers;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Application.DTOs.Requests;
using Application.DTOs;

namespace Application;

public class AuthenticationService : IAuthenticationService
{
    /// <inheritdoc/>
    /// <remarks>
    ///     The method will never throw an exception.
    ///     <list type="bullet">
    ///         <item>
    ///             <term>ERROR</term>
    ///             <description>Error that may contain sensitive information that shouldn't be sent to the frontend</description>
    ///         </item>
    ///         <item>
    ///             <term>WARNING</term>
    ///             <description>Safe"" error messages that has a certain user mistake associated, and can therefore be shown to anyone</description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public bool Login(LoginRequest request, out string token_result)
    {
        try
        {
            _loginValidator.ValidateAndThrow(request);

            var user = FindUser(request.Username); 

            if (user == null)
            {
                token_result = "WARNING: User not found";
                return false;
            }

            if (HashGenerator.Validate(request.Password, user.Salt, user.HashedPassword))
            {
                token_result = GenerateToken(user, _secret);
                return true;
            }

            token_result = "WARNING: User could not be authenticated";
            return false;
        }
        catch (ArgumentOutOfRangeException ae)
        {
            token_result = "ERROR: JWT Generation error ->" + ae.Message;
            return false;
        }
        catch (Exception e)
        {
            token_result = "ERROR: " + e.Message;
            return false;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     The method will never throw an exception.
    ///     <list type="bullet">
    ///         <item>
    ///             <term>ERROR</term>
    ///             <description>Error that may contain sensitive information that shouldn't be sent to the frontend</description>
    ///         </item>
    ///         <item>
    ///             <term>WARNING</term>
    ///             <description>Safe"" error messages that has a certain user mistake associated, and can therefore be shown to anyone</description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public bool Register(RegisterRequest request, out string token_result)
    {
        try
        {
            _registerValidator.ValidateAndThrow(request);

            var user = FindUser(request.Username);

            if (user != null)
            {
                token_result = "WARNING: User already exists";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.DisplayName))
            {
                request.DisplayName = request.Username;
            }


            user = _userRepository.Create(ObjectGenerator.GenerateUser(request)) ?? throw new Exception("user could not be created!");
            token_result = GenerateToken(user, _secret);

            return true;
        }
        catch (ArgumentOutOfRangeException ae)
        {
            token_result = "ERROR: JWT Generation error ->" + ae.Message;
            return false;
        }
        catch (Exception e)
        {
            token_result = "ERROR: " + e.Message;
            return false;
        }
    }

    /// <summary>
    ///     Finds a user by username.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <returns>The user or null if not found.</returns>
    /// <remarks>
    ///     This method is used by 
    ///     <list>
    ///         <item><see cref="Login"/></item>
    ///         <item><see cref="Register"/></item>
    ///     </list>
    /// </remarks>
    /// <completionlist cref="(User, ObjectGenerator, AuthenticationService)"/>
    /// <author>
    ///     <name>Mads Mandahl-Barth</name>
    /// </author>
    private User? FindUser(string username)
    {
        try
        {
            if (Configuration.IsDebug && username == "debug")
            {
                return ObjectGenerator.GenerateUser(new RegisterRequest
                {
                    Username = "debug",
                    DisplayName = "Debug User",
                    Password = "debug"
                });
            }

            return _userRepository.Find(username);
        }
        catch
        {
            return null;
        }
    }
    public bool AuthenticateToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_secret),
        };

        try
        {
            handler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public TokenUser? GetPartialUserFromToken(string token)
    {
        try
        {
            var payload = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return FindUser(payload.Claims.First(c => c.Type == "username").Value).ToTokenUser();
        }
        catch
        {
            return null;
        }
    }

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
    public string GenerateToken(User user, byte[] secret)
    {
        List<Claim> claims = new()
        {
            new Claim("id", user.Id.ToString()),
            new Claim("name", user.DisplayName ?? user.Username),
            new Claim("username", user.Username),
        };

        var payload = new JwtPayload(null, null, claims, DateTime.Now, DateTime.Now.AddMinutes(45));
        var header = new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha512));
        var token = new JwtSecurityToken(header, payload);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public AuthenticationService(
        IUserRepository userRepository,
        IValidator<LoginRequest> loginValidator, 
        IValidator<RegisterRequest> registerValidator, 
        IValidator<User> userValidator, 
        byte[] secret)
    {
        _userRepository = userRepository;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
        _validator = userValidator;
        _secret = secret;
    }
    private readonly byte[] _secret;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<User> _validator;
}
