namespace API.Helper
{
    public class LikesParam : PaginationParam
    {
        public int UserId { get; set; }
        public required string Predicate { get; set; } = "liked";
    }
}
