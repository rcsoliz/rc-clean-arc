using Application.Common;
using Application.Interfaces;
using Core.Dtos;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPostByUserId
{
    public class GetAllPostByUserIdQueryHandler : IRequestHandler<GetAllPostByUserIdQuery, PagedResult<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        public GetAllPostByUserIdQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<PagedResult<PostDto>> Handle(GetAllPostByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _postRepository.GetAllPostByUserId(request.id);
        }
    }
}
