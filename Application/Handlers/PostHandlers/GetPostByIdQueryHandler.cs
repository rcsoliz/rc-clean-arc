using Application.Interfaces;
using Application.Queries.PostCommands;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Handlers.PostHandlers
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Post>
    {
        private readonly IPostRepository _postRepository;

        public GetPostByIdQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<Post> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            return await _postRepository.GetByIdAsync(request.id);
        }
    }
}
