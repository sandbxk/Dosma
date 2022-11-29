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
    /*
        * ValidateLogin
        * 
        * Validates the login credentials of a user.
        * 
        * @param username The username of the user.
        * @param password The password of the user.
        * @param result The token of the user if validated, otherwise an error message.
        * 
        * @return True if the login credentials are valid, false otherwise.
    */
    public bool Login(LoginRequestDTO login, out string result)
    {
        try
        {
            _loginValidator.ValidateAndThrow(login);
            #pragma warning disable CS8604  // Possible null reference argument.
                                            // This is not possible because of the validation above.
            User user = FindUser(login.Username); 
            if (HashGenerator.Validate(login.Password, user.Salt, user.HashedPassword))
            {
                result = TokenGenerator.GenerateToken(user, _secret);
                return true;
            }
            #pragma warning restore CS8604
            result = "User could not be authenticated";
            return false;
        }
        catch (ArgumentOutOfRangeException ae)
        {
            throw new InvalidProgramException("JWT Generation error", ae);
        }
        catch (System.Exception e)
        {
            result = e.Message;
            return false;
        }
    }
    public bool Register(RegisterRequestDTO registration, out string result)
    {
        throw new NotImplementedException();
    }
    private User FindUser(string username)
    {
        #region DEBUG
        /*
            DEBUG is removed during RELEASE compilation, so this code will not be included in the release build.
        */
        #if DEBUG
        if (username == "debug")
        {
            return ObjectGenerator.GenerateUser(new RegisterRequestDTO
            {
                Username = "debug",
                DisplayName = "Debug User",
                Password = "debug"
            });
        }
        #endif
        #endregion
        return _userRepository.Find(username);
    }
    public AuthenticationService(
        IUserRepository userRepository, 
        IMapper mapper, 
        IValidator<LoginRequestDTO> loginValidator, 
        IValidator<User> userValidator, 
        byte[] secret)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _loginValidator = loginValidator;
        _validator = userValidator;
        _secret = secret;
    }
    private readonly byte[] _secret;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<LoginRequestDTO> _loginValidator;
    private readonly IValidator<User> _validator;
}
