using Application.DTOs;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetByPostWithCategoriesById
{
    public record GetByPostWithCategoriesByIdQuery(int id) : IRequest<IEnumerable<PostWithCategoriesDto>>;
}
