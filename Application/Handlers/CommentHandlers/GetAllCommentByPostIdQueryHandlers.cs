using Application.Queries.CommentCommands;
using Core.Dtos;
using Core.Interfaces;
using MediatR;

namespace Application.Handlers.CommentHandlers
{
    public class GetAllCommentByPostIdQueryHandlers : IRequestHandler<GetAllCommentByPostIdQuery, IEnumerable<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetAllCommentByPostIdQueryHandlers(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<CommentDto>> Handle(GetAllCommentByPostIdQuery request, CancellationToken cancellationToken)
        {
            return await _commentRepository.GetAllCommentByPostId(request.Id);
        }
    }
}
