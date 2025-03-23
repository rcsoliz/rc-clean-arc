
namespace Core.Dtos
{
    public class PostFilterDto
    {
        public string? SearchText { get; set; }         // Por contenido
        public string? Username { get; set; }            // Por autor
        public DateTime? StartDate { get; set; }         // Fecha inicial
        public DateTime? EndDate { get; set; }           // Fecha final
        public int Page { get; set; } = 1;               // Paginación
        public int PageSize { get; set; } = 10;
    }
}
