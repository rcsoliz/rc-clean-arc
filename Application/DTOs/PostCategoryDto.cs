namespace Application.DTOs
{
    public class PostCategoryDto
    {
        public int PostId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
