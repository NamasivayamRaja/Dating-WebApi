using API.Entities;

namespace API.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public int Age { get; set; }
        public string? PhotoUrl { get; set; }
        public required string KnownAs { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastActive { get; set; }
        public int Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public List<PhotoDTO> Photos { get; set; } = [];
    }
}
