using Application.DTOs;
using Core.Entities;

namespace Application.Interfaces
{
    public interface IPostCategoryRepository
    {
        Task AddAsync(Post entity, List<int> categoryIds, CancellationToken cancellationToken);

        Task UpdateAsync(Post entity, List<int> categoryIds, CancellationToken cancellationToken);

        Task DeleteAsync(int id, List<int> categoryIds, CancellationToken cancellationToken);

        Task<IEnumerable<PostDto>> GetAllPostsWithCategoriesAsync(CancellationToken cancellationToken);

        Task<PostDto> GetPostByIdtWithCategoriesAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<PostDto>> GetPostWithCategoryIdAsync(int categoryId, CancellationToken cancellationToken);

        Task<IEnumerable<PostWithCategoriesDto>> GetPostsByScrollAsync(DateTime? lastPostDate, int take = 5, CancellationToken cancellationToken = default);

        Task<int> CountNewPostsAsync(DateTime afterDate, CancellationToken cancellationToken);
        Task<IEnumerable<PostWithCategoriesDto>> GetNewPostsAfterAsync(DateTime afterDate, CancellationToken cancellationToken);

    }
}
