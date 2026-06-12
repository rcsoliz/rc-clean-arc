using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto>
    {
        private readonly IPostRepository _postRepository;

        public CreatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                PostContent = request.PostContent,
                UserId = request.UserId,
                ImageUrl = request.ImageUrl
            };

            await _postRepository.AddAsync(post, cancellationToken);

            return new PostDto
            {
                Id = post.Id,
                PostContent = post.PostContent,
                UserId = post.UserId
            };
        }
    }
}
