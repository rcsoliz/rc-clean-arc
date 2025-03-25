using Application.Common;
using Application.DTOs;
using Core.Entities;

namespace Application.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllAsync();
        Task<PostDto> GetByIdAsync(int id);
        Task AddAsync(Post entity);

        Task<IEnumerable<PostDto>> GetAllPostWithDetailsAsync();

        Task<List<PostDto>> GetPagedPostsAsync(int page, int pageSize);

        Task<PagedResult<PostDto>> GetPagedPostsAsyncRefactory(int page, int pageSize);
        IQueryable<Post> GetQueryableWithUserAndComments();

        Task<PagedResult<PostDto>> GetAllPostByUserId(int id);
    }
}
