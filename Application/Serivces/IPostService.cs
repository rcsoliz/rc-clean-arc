using Application.DTOs;

namespace Application.Serivces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllDetailedPostsAsync();
        Task<List<PostDto>> GetPagedPostsAsync(int page, int pageSize);
    }
}
