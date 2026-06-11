

namespace Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty; 
        public ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();

    }
}
