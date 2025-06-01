namespace API.DTOs
{
    public class UserDTO
    {
        public required string UserName { get; set; }
        public required string Token { get; set; }
        public string? PhotoUrl { get; set; }
        public string? KnownAs { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
    }
}
