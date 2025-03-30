using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Post>
    {
        private readonly IPostRepository _postRepository;

        public CreatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                PostContent = request.PostContent,
                UserId = Convert.ToInt32(request.UserId)

            };

            await _postRepository.AddAsync(post);
            return post;
        }
    }
}
