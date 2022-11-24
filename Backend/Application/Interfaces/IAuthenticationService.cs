namespace Application.Interfaces;

public interface IAuthenticationService
{
    public bool ValidateLogin(string userName, string password, out string token);

    public bool ValidateToken(string token);
}
