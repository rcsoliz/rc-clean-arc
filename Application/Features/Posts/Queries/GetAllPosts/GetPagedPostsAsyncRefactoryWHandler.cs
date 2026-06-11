using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPosts
{
    public class GetPagedPostsAsyncRefactoryHandler : IRequestHandler<GetPagedPostsAsyncRefactoryQuery, PagedResult<PostDto>>
    {
        private readonly IPostRepository _postRepository;

        public GetPagedPostsAsyncRefactoryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<PagedResult<PostDto>> Handle(GetPagedPostsAsyncRefactoryQuery request, CancellationToken cancellationToken)
        {
            return await _postRepository.GetPagedPostsAsyncRefactory(request.Page, request.PageSize,cancellationToken);
        }
    }
}
