using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    public bool ValidateLogin(string username, string password, out string token);

    public bool ValidateToken(string token);
}
