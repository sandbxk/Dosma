using System.Text;
using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;
using Application.Helpers;

namespace Backend.Application
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ValidateLogin(string username, string password, out string token)
        {
            User user = _userRepository.Find(username);

            if (HashGenerator.Validate(password, user.Salt, user.HashedPassword))
            {
                token = TokenGenerator.GenerateToken(user);
                return true;
            }

            token = string.Empty;
            return false;
        }


        public bool ValidateToken(string token, string[] claims)
        {
            // TODO: does user exist?
            // TODO: does user have claims?

            return false;
        }
    }
}