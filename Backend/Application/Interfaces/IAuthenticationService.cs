using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    // @param result The token of the user if validated, otherwise an error message.
    public bool Login(LoginRequestDTO login, out string token_result);
    
    // @param result The token of the user if validated, otherwise an error message.
    public bool Register(RegisterRequestDTO registration, out string token_result);
}
