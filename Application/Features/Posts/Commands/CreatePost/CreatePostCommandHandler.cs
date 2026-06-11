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
            var post = new PostDto
            {
                PostContent = request.PostContent,
                UserId = Convert.ToInt32(request.UserId)

            };

            await _postRepository.AddAsync(post, cancellationToken);

            return post;
        }
    }
}
