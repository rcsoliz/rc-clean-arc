using Application.DTOs;
using Core.Entities;

namespace Application.Interfaces
{
    public interface IPostCategoryRepository
    {
        Task AddAsync(Post entity, List<int> categoryIds);

        Task UpdateAsync(Post entity, List<int> categoryIds);

        Task DeleteAsync(int id, List<int> categoryIds);

        
        Task<(PostDto Post, IEnumerable<PostCategoryDtos> Categories)> GetByIDAsync(int id);
    }
}
