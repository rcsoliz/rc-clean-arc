using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Categories.Queries.GatAllCategories
{
    public class GetAllCategoriesQueryHandlers : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandlers(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
                return await _categoryRepository.GetAllAsync();
        }
    }
}
