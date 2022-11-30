namespace Application.DTOs;

public class RegisterRequest 
{
    public string DisplayName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}