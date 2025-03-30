using Application.Features.Posts.Commands.CreatePostRefactory;
using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Posts.Commands.CreatePostCategory
{
    public class CreatePostCategoryCommandHandler : IRequestHandler<CreatePostCategoryCommand, (Post post, List<int> categories)>
    {
        private readonly IPostRepository _repository;

        public CreatePostCategoryCommandHandler(IPostRepository repository)
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

            await _repository.AddAsyncRefactory(post, request.CategoryIds);

            return (post, request.CategoryIds);
        }
    }
}
