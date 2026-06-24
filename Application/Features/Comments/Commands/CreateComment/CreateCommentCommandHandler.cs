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
        private readonly INotificationService _notificationService;
        private readonly IPostRepository _postRepository;

        public CreateCommentCommandHandler(ICommentRepository commentRepository, IUserRepository userRepository, 
                INotificationService notificationService, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _postRepository = postRepository;
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

            var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
            if (post != null && post.UserId != request.UserId)
            {
                await _notificationService.SendNotificationAsync(
                    toUserId: post.UserId,
                    type: "comment",
                    message: $"{user?.Username} comentó en tu publicación",
                    postId: request.PostId,
                    cancellationToken: cancellationToken
                );
            }

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