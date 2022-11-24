using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;
using Application.Helpers;
using AutoMapper;
using FluentValidation;
using Application.DTOs;

namespace Backend.Application
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly byte[] _secret;
        private readonly IUserRepository _userRepository;
        
        private IMapper _mapper;

        private IValidator<LoginRequestDTO> _loginValidator;
        private IValidator<User> _validator;

        public AuthenticationService(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<LoginRequestDTO> loginValidator,
            IValidator<User> userValidator,
            byte[] secret
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _loginValidator = loginValidator;
            _validator = userValidator;
            _secret = secret;
        }

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
        public bool ValidateLogin(LoginRequestDTO login, out string result)
        {
            try
            {
                User user = FindUser(login.Username);

                if (HashGenerator.Validate(login.Password, user.Salt, user.HashedPassword))
                {
                    result = TokenGenerator.GenerateToken(user, _secret);
                    return true;
                }

                result = "User could not be authenticated";
                return false;
            }
            catch (System.Exception e)
            {
                result = e.Message;
                return false;
            }
        }


        #region DEBUG

        private User FindUser(string username)
        {
            /*
                DEBUG is removed during RELEASE compilation, so this code will not be included in the release build.
            */
            #if DEBUG
            if (username == "debug")
            {
                return DebugUser();
            }
            else
            {
                return _userRepository.Find(username);
            }
            #else
            /*
                This code will be included in the final build.
            */
            return _userRepository.Find(username);
            #endif
        }

        #if DEBUG // Make sure that this code is not accidentally used elsewhere making it in the release build.
        private User DebugUser()
        {
            (var salt, var hash) = HashGenerator.Generate("debug");

            return new User
            {
                Username = "debug",
                DisplayName = "Debug User",
                HashedPassword = hash,
                Salt = salt
            };
        }
        #endif

        #endregion
    }
}