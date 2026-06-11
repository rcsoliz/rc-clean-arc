using Application.DTOs;
using MediatR;

namespace Application.Features.Categories.Queries.GetAllCategories
{
    public record GetAllCategoriesQuery: IRequest<IEnumerable<CategoryDto>>;

}
