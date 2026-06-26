namespace Application.DTOs
{
    public class UpdateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
