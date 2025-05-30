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
        [Required]
        public string? DateOfBirth { get; set; }
        [Required]
        public string? KnownAs { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Country { get; set; }
    }
}
