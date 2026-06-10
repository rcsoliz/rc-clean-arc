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
            var posts = await _postRepository.GetAllAsync();
            var list = new List<PostDto>();
            foreach (var post in posts)
            {
                list.Add(new PostDto
                {
                    Id = post.Id,
                    PostContent = post.PostContent,
                    UserId = post.UserId
                });
            }
            return list;
        }
    }
}
