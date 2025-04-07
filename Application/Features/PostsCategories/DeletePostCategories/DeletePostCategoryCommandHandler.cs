using Application.Interfaces;
using MediatR;

namespace Application.Features.PostsCategories.DeletePostCategories
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
            var post = await _repository.GetByIDAsync(request.id);
            if (post.Post == null) return false;

            await _repository.DeleteAsync(request.id, request.categoryIds);
            return true;
        }
    }
}
