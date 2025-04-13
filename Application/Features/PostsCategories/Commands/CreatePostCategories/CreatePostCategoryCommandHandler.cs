using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.PostsCategories.Commands.CreatePostCategories
{
    public class CreatePostCategoryCommandHandler : IRequestHandler<CreatePostCategoryCommand, (Post post, List<int> categories)>
    {
        private readonly IPostCategoryRepository _repository;

        public CreatePostCategoryCommandHandler(IPostCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<(Post post, List<int> categories)> Handle(CreatePostCategoryCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                PostContent = request.PostContent,
                UserId = int.Parse(request.UserId),
                ImageUrl = request.ImageUrl,
            };

            await _repository.AddAsync(post, request.CategoryIds);

            return (post, request.CategoryIds);
        }
    }
}
