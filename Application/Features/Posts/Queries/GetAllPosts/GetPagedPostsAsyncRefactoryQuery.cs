using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPosts
{
    public class GetPagedPostsAsyncRefactoryQuery : IRequest<PagedResult<PostDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public GetPagedPostsAsyncRefactoryQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
