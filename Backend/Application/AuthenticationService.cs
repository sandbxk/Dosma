using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;

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
            token = string.Empty;
            return false;
        }

        public bool ValidateToken(string token)
        {
            return false;
        }
    }
}