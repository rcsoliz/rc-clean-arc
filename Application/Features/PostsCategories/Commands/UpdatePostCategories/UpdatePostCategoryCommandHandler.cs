using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.PostsCategories.Commands.UpdatePostCategories
{
    public class UpdatePostCategoryCommandHandler : IRequestHandler<UpdatePostCategoryCommand, bool>
    {
        private readonly IPostCategoryRepository _postRepository;

        public  UpdatePostCategoryCommandHandler(IPostCategoryRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<bool> Handle(UpdatePostCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingPost = await _postRepository.GetPostByIdtWithCategoriesAsync(request.Id, cancellationToken);
            if (existingPost == null) return false;

            var post = new Post
            {
                Id = request.Id,
                PostContent = request.PostContent,
                UserId = request.UserId,
                ImageUrl = request.ImageUrl,
            };
            await _postRepository.UpdateAsync(post, request.CategoryIds, cancellationToken);

            return true;
        }

    }
}
