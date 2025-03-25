using Application.Interfaces;
using Application.Queries.CommentCommands;
using Core.Entities;
using MediatR;

namespace Application.Handlers.CommentHandlers
{
    public class GetCommentsQueryHandlers : IRequestHandler<GetAllCommentQuery, IEnumerable<Comment>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentsQueryHandlers(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<IEnumerable<Comment>> Handle(GetAllCommentQuery request, CancellationToken cancellationToken)
        {
            return await _commentRepository.GetAllAsync();
        }
    }
}
