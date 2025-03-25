using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Comments.Queries.GetAllComments
{
    public class GetAllCommentsQueryHandlers : IRequestHandler<GetAllCommentQuery, IEnumerable<Comment>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetAllCommentsQueryHandlers(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<IEnumerable<Comment>> Handle(GetAllCommentQuery request, CancellationToken cancellationToken)
        {
            return await _commentRepository.GetAllAsync();
        }
    }
}
