using Application.DTOs;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetAllPostWithCategoryId
{
    public record GetPostWithCategoryIdQuery(int categoryId) : IRequest<PostWithCategoriesDto>;
}
