using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public  string Password { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public required string KnownAs { get; set; }
        public int Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public int LookingFor { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public List<Photo> Photos { get; set; } = [];
    }
}
