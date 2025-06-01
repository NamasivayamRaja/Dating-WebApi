namespace API.Helper
{
    public class PaginationHeader(int currentPage,int itemPerPage, int totalItems, int totalPages)
    {
        public int CurrentPage { get; set; } = currentPage;
        public int ItemsPerPage { get; set; } = itemPerPage;
        public int TotalItems { get; set; } = totalItems;
        public int TotalPages { get; set; } = totalPages;
    }
}
