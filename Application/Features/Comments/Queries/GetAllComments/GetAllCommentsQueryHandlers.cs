using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Comments.Queries.GetAllComments
{
    public class GetAllCommentsQueryHandlers : IRequestHandler<GetAllCommentQuery, IEnumerable<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetAllCommentsQueryHandlers(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<IEnumerable<CommentDto>> Handle(GetAllCommentQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetAllAsync(cancellationToken);
            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                CommentContent = c.CommentContent,
                Username = c.User.Username ?? string.Empty,
                UserId = c.UserId,
                PostId = c.PostId,
                Created = c.CreatedAt.ToString("s")
            });
        }
    }
}
