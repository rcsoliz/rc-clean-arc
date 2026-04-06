using Application.DTOs;
using Core.Entities;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryById(int Id) : IRequest<Category>;
}
