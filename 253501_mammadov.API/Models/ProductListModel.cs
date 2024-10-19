namespace _253501_mammadov.API.Models
{
    public class ListModel<T>
    {
        public List<T> Items { get; set; } = new();

        public int TotalCount { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; } = 1;

        public int PageSize { get; set; } = 1;
    }
}
