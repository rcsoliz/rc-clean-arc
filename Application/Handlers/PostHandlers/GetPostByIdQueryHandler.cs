using Application.Interfaces;
using Application.Queries.PostCommands;
using Core.Dtos;
using MediatR;

namespace Application.Handlers.PostHandlers
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDto>
    {
        private readonly IPostRepository _postRepository;

        public GetPostByIdQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            return await _postRepository.GetByIdAsync(request.id);
        }

    }
}
