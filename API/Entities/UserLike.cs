namespace API.Entities
{
    public class UserLike
    {
        public int SourceUserId { get; set; }
        public AppUser SourceUser { get; set; } = null!;
        public int TargetUserId { get; set; }
        public AppUser TargetUser { get; set; } = null!;
        // This is used to prevent duplicate likes in the database
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // This is used to prevent duplicate likes in the database
        public string Key => $"{SourceUserId}-{TargetUserId}";
    }
}
