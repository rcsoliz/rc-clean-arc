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
        public async Task<IEnumerable<PostDto>> GetAllDetailedPostsAsync(CancellationToken cancellationToken)
        {
            return await _postRepository.GetAllPostWithDetailsAsync(cancellationToken);
        }

        public async Task<List<PostDto>> GetPagedPostsAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _postRepository.GetPagedPostsAsync(page, pageSize, cancellationToken);
        }
    }
}
