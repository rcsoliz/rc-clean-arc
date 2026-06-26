using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPostWithDetails
{
    public class GetAllPostWithDetailsQueryHandler : IRequestHandler<GetAllPostWithDetailsQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository;

        public GetAllPostWithDetailsQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetAllPostWithDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _postRepository.GetAllPostWithDetailsAsync(cancellationToken);
        }
    }
}
