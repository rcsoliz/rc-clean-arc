using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Comments.Queries.GetCommentById
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentDto?>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentByIdQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<CommentDto?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (comment == null) return null;
            return new CommentDto
            {
                Id = comment.Id,
                CommentContent = comment.CommentContent,
                Username = comment.User?.Username?? string.Empty,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Created = comment.CreatedAt.ToString("s")
            };
        }
    }
}
