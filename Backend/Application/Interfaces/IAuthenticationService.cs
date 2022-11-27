using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    public bool ValidateLogin(LoginRequestDTO login, out string result);
    public bool ValidateRegister(RegisterRequestDTO registration, out string result);
}
