using Application.DTOs;
using Application.Interfaces;

namespace Application.Serivces
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<IEnumerable<PostDto>> GetAllDetailedPostsAsync()
        {
            return await _postRepository.GetAllPostWithDetailsAsync();
        }

        public async Task<List<PostDto>> GetPagedPostsAsync(int page, int pageSize)
        {
            return await _postRepository.GetPagedPostsAsync(page, pageSize);
        }
    }
}
