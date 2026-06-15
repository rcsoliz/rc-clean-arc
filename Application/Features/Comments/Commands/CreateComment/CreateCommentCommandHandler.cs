using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDto>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public CreateCommentCommandHandler(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
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

            await _commentRepository.AddAsync(comment, cancellationToken);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            return new CommentDto
            {
                Id = comment.Id,
                CommentContent = comment.CommentContent,
                Username = user?.Username ?? string.Empty,
                UserId = comment.UserId,
                PostId = comment.PostId,
                ParentCommentId = comment.ParentCommentId,
                Created = comment.CreatedAt.ToString("s")
            };
        }
    }
}