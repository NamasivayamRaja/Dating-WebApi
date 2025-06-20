﻿using API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
        public DateOnly DateOfBirth { get; set; }
        public required string KnownAs { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public int Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public List<Photo> Photos { get; set; } = [];

        // Navigation Properties
        public List<UserLike> LikedUsers { get; set; } = [];
        public List<UserLike> LikedByUsers { get; set; } = [];

        public List<Message> MessagesSend { get; set; } = [];
        public List<Message> MessagesReceived { get; set; } = [];

        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}
