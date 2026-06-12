using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.PostsCategories.Commands.CreatePostCategories
{
    public class CreatePostCategoryCommandHandler : IRequestHandler<CreatePostCategoryCommand, PostDto>
    {
        private readonly IPostCategoryRepository _repository;

        public CreatePostCategoryCommandHandler(IPostCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PostDto> Handle(CreatePostCategoryCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                PostContent = request.PostContent,
                UserId = request.UserId,
                ImageUrl = request.ImageUrl,
            };

            await _repository.AddAsync(post, request.CategoryIds, cancellationToken);

            return new PostDto
            {
                Id = post.Id,
                PostContent = post.PostContent,
                UserId = post.UserId
            };
        }
    }
}
