using Application.DTOs;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetNewPostsAfterAsync
{
    public record GetNewPostsAfterAsyncQuery(DateTime afterDate) : IRequest<IEnumerable<PostWithCategoriesDto>>;
}
