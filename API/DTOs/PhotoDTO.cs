namespace API.DTOs
{
    public class PhotoDTO
    {
        public required string Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }

    }
}