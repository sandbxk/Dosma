using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;
using Application.Helpers;
using AutoMapper;
using FluentValidation;
using Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Application;

public class AuthenticationService : IAuthenticationService
{
    /// <inheritdoc/>
    public bool Login(LoginRequest request, out string token)
    {
       // if (!_loginValidator.Validate(request).IsValid)
       // {
       //     throw new ArgumentException("Invalid login request");
       // }
        
        var user = FindUser(request.Username); 
        
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        token = "";

        try
        {
            if (HashGenerator.Validate(request.Password, user.Salt, user.HashedPassword))
            {
                token = GenerateToken(_mapper.Map<TokenUserDTO>(user), _secret);
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            // server error - rethrow any as InvalidProgramException

            throw new InvalidProgramException("JWT Generation error", e);
        }
    }

    /// <inheritdoc/>
    public bool Register(RegisterRequest request, out string token)
    { 
       // if (_registerValidator.Validate(request).IsValid)
       // {
       //     throw new ArgumentException("Invalid registration request");
       // }

        var user = FindUser(request.Username);
        
        if (user != null)
        {
            throw new ArgumentException("User already exists");
        }
        
        if (string.IsNullOrWhiteSpace(request.DisplayName))
        {
            request.DisplayName = request.Username;
        }

        token = "";

        try
        {
            user = _userRepository.Create(ObjectGenerator.GenerateUser(request));

            token = GenerateToken(_mapper.Map<TokenUserDTO>(user), _secret);
            return true;
        }
        catch (Exception e)
        {
            // server error - rethrow any as InvalidProgramException

            throw new InvalidProgramException("Error creating new user", e);
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

    public User? GetUserFromToken(string token)
    {
        try
        {
            var payload = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return FindUser(payload.Claims.First(c => c.Type == "username").Value);
        }
        catch
        {
            return null;
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
    public string GenerateToken(TokenUserDTO user, byte[] secret)
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
        IMapper mapper, 
        IValidator<LoginRequest> loginValidator, 
        IValidator<RegisterRequest> registerValidator, 
        IValidator<User> userValidator, 
        byte[] secret)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
        _validator = userValidator;
        _secret = secret;
    }
    private readonly byte[] _secret;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<User> _validator;
}
