namespace Application.DTOs
{
    public class TokenDTO
    {
        public uint Status { get; set; } = 200;
        public string? ErrorMessage { get; set; }
        public string Token { get; set; } = null!;
    }
}

