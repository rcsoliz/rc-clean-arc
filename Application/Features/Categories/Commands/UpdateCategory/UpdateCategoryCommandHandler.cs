using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Category>
    {
        private readonly ICategoryRepository _categoryRepository;
        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<Category> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new ApplicationException($"Category with Id={request.Id} not found");
            }

            var itemCategory = new Category
            {
                Id = request.Id,
                Name = request.Name
            };
            await _categoryRepository.UpdateAsync(itemCategory);
            return itemCategory;
        }
    }
}
