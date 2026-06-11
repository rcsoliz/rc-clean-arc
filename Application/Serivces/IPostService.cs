using Application.DTOs;

namespace Application.Serivces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllDetailedPostsAsync(CancellationToken cancellationToken);
        Task<List<PostDto>> GetPagedPostsAsync(int page, int pageSize, CancellationToken cancellationToken);
    }
}
