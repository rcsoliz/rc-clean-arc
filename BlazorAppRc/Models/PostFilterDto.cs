namespace BlazorAppRc.Models
{
    public class PostFilterDto
    {
        public string? SearchText { get; set; }
        public string? Username { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
