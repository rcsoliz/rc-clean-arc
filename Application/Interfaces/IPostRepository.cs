using Application.Common;
using Application.DTOs;
using Core.Entities;

namespace Application.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllAsync(CancellationToken cancellation);
        Task<PostDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task AddAsync(Post entity, CancellationToken cancellationToken);


        Task<IEnumerable<PostDto>> GetAllPostWithDetailsAsync(CancellationToken cancellationToken);

        Task<List<PostDto>> GetPagedPostsAsync(int page, int pageSize, CancellationToken cancellationToken);

        Task<PagedResult<PostDto>> GetPagedPostsAsyncRefactory(int page, int pageSize, CancellationToken cancellationToken);
        IQueryable<Post> GetQueryableWithUserAndComments(CancellationToken cancellationToken);

        Task<PagedResult<PostDto>> GetAllPostByUserId(int id, CancellationToken cancellationToken);
    }
}
