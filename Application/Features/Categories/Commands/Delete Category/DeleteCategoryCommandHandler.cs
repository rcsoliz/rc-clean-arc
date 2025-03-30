using Application.Interfaces;
using MediatR;

namespace Application.Features.Categories.Commands.Delete_Category
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.id);
            if (category == null) return false;

            await _categoryRepository.DeleteAsync(category.Id);
            return true;
        }
    }
}
