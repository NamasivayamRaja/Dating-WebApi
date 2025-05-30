﻿namespace API.DTOs
{
    public class UserDTO
    {
        public required string UserName { get; set; }
        public required string Token { get; set; }
        public string? PhotoUrl { get; set; }
        public string? KnownUs { get; set; }
    }
}
