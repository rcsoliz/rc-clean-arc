using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPosts
{
    public class GetPagedPostsQuery : IRequest<PagedResult<PostDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
