using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;
using Application.Helpers;
using AutoMapper;
using FluentValidation;
using Application.DTOs;

namespace Application;

public class AuthenticationService : IAuthenticationService
{
    /// <inheritdoc/>
    public bool Login(LoginRequest request, out string token)
    {
        if (!_loginValidator.Validate(request).IsValid)
        {
            throw new ArgumentException("Invalid login request");
        }
        
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
                token = TokenGenerator.GenerateToken(user, _secret);
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
        if (_registerValidator.Validate(request).IsValid)
        {
            throw new ArgumentException("Invalid registration request");
        }

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

            token = TokenGenerator.GenerateToken(user, _secret);
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
        return TokenGenerator.ValidateToken(token, _secret);
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
