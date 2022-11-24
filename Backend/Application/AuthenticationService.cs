using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;

namespace Backend.Application
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<User> _authenticationRepository;

        public AuthenticationService(IRepository<User> authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public bool ValidateLogin(string userName, string password, out string token)
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