using Application.DTOs;
using MediatR;

namespace Application.Features.PostsCategories.Queries.GetPostsByScroll
{
    public record GetPostsByScrollQuery(DateTime? LastPostDate, int Take = 5) : IRequest<IEnumerable<PostWithCategoriesDto>>;
 
}
