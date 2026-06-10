using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPosts
{
    public class GetPagedPostsQueryHandler : IRequestHandler<GetPagedPostsQuery, PagedResult<PostDto>>
    {
        private readonly IPostRepository _postRepository;

        public GetPagedPostsQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<PagedResult<PostDto>> Handle(GetPagedPostsQuery request, CancellationToken cancellationToken)
        {
            return await _postRepository.GetPagedPostsAsyncRefactory(request.Page, request.PageSize);
        }
    }
}
