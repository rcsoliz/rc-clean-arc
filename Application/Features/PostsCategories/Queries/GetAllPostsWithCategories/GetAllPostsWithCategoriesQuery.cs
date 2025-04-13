using Application.DTOs;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetAllPostsWithCategories
{
    public record GetAllPostsWithCategoriesQuery() : IRequest<IEnumerable<PostWithCategoriesDto>>;
}
