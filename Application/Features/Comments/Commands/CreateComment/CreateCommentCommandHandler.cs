using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Core.Models;
using MediatR;

namespace Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDto>
    {
        private readonly ICommentRepository _commentRepository;

        public CreateCommentCommandHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                CommentContent = request.CommentContent,
                UserId = request.UserId,
                PostId = request.PostId,
                ParentCommentId = request.ParentCommentId
            };

            await _commentRepository.AddAsync(comment,cancellationToken);
            return new CommentDto
            {
                Id = comment.Id,
                CommentContent = comment.CommentContent,
                Username = comment.User.Username,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Created = comment.CreatedAt.ToString("s")
            };
        }
    }
}
