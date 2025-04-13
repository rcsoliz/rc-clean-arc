using Application.DTOs;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetByPostWithCategoriesById
{
    public record GetPostByIdWithCategoriesQuery(int id) : IRequest<PostWithCategoriesDto>;
}
