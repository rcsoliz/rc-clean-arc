using Application.Interfaces;
using MediatR;

namespace Application.Features.PostsCategories.Commands.DeletePostCategories
{
    public class DeletePostCategoryCommandHandler : IRequestHandler<DeletePostCategoryCommand, bool>
    {
        private readonly IPostCategoryRepository _repository;

        public DeletePostCategoryCommandHandler(IPostCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeletePostCategoryCommand request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetPostByIdtWithCategoriesAsync(request.id);
            if (post == null) return false;

            await _repository.DeleteAsync(request.id, request.categoryIds);
            return true;
        }
    }
}
