namespace API.Helper
{
    public class MessageParams : PaginationParam
    {
        public string? UserName { get; set; }
        public string Container { get; set; } = "Unread";
    }
}
