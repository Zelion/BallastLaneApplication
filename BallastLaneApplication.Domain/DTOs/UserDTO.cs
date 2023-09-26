namespace BallastLaneApplication.Domain.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? HashPassword { get; set; }
        public byte[]? Salt { get; set; }
    }
}
