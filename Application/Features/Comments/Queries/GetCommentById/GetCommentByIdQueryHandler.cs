using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Comments.Queries.GetCommentById
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, Comment>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentByIdQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<Comment> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _commentRepository.GetByIdAsync(request.Id);
        }
    }
}
