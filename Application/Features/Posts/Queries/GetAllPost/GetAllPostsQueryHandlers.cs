using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPost
{
    public class GetAllPostsQueryHandlers : IRequestHandler<GetAllPostQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository;

        public GetAllPostsQueryHandlers(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllAsync(cancellationToken);
            return posts.Select(p => new PostDto
            {
                Id = p.Id,
                PostContent = p.PostContent,
                Username = p.User?.Username ?? string.Empty,
                UserId = p.UserId,
                Created = p.CreatedAt.ToString("s")
            });
        }
    }
}
