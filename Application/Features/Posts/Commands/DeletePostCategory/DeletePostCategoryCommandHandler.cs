using Application.Interfaces;
using MediatR;

namespace Application.Features.Posts.Commands.DeletePostCategory
{
    public class DeletePostCategoryCommandHandler : IRequestHandler<DeletePostCategoryCommand, bool>
    {
        private readonly IPostRepository _repository;

        public DeletePostCategoryCommandHandler(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeletePostCategoryCommand request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetByIdAsync(request.id);
            if (post == null) return false;

            await _repository.DeleteAsync(request.id, request.categoryIds);
            return true;
        }
    }
}
