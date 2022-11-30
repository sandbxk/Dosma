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
                token_result = TokenGenerator.GenerateToken(user, _secret);
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

            user = _userRepository.Create(ObjectGenerator.GenerateUser(request));

            token_result = TokenGenerator.GenerateToken(user, _secret);
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
            if (Configuration.isDebug && username == "debug")
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
