using Application.DTOs;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryById(int Id) : IRequest<CategoryDto>;
}
