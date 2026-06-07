using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdHandlers : IRequestHandler<GetCategoryById, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdHandlers(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryDto> Handle(GetCategoryById request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (category == null) return null;
            
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
