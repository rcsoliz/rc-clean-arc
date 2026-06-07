using Application.DTOs;
using MediatR;

namespace Application.Features.Categories.Queries.GatAllCategories
{
    public record GetAllCategoriesQuery: IRequest<IEnumerable<CategoryDto>>;

}
