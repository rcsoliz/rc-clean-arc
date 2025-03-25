using Application.Features.Posts.Queries.GetAllPost;
using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPosts
{
    public class GetAllPostsQueryHandlers : IRequestHandler<GetAllPostQuery, IEnumerable<Post>>
    {
        private readonly IPostRepository _postRepository;

        public GetAllPostsQueryHandlers(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<Post>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            return await _postRepository.GetAllAsync();
        }
    }
}
