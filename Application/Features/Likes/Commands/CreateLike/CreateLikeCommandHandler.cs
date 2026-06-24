using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Likes.Commands.CreateLike
{
    public class CreateLikeCommandHandler : IRequestHandler<CreateLikeCommand, LikeDto?>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IPostRepository _postRepository;
        private readonly INotificationService _notificationService;

        public CreateLikeCommandHandler(
            ILikeRepository likeRepository,
            IPostRepository postRepository,
            INotificationService notificationService)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
            _notificationService = notificationService;
        }

        public async Task<LikeDto?> Handle(CreateLikeCommand request, CancellationToken cancellationToken)
        {
            var like = new Like
            {
                UserId = request.UserId,
                PostId = request.PostId,
                CommentId = request.CommentId,
            };
            await _likeRepository.AddAsync(like, cancellationToken);

            if (request.PostId>0)
            {
                var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
                if (post != null && post.UserId != request.UserId)
                {
                    await _notificationService.SendNotificationAsync(
                        toUserId: post.UserId,
                        type: "like",
                        message: "A alguien le gustó tu publicación",
                        postId: request.PostId,
                        cancellationToken: cancellationToken
                    );
                }
            }

            return new LikeDto
            {
                Id = like.Id,
                UserId = like.UserId,
                PostId = like.PostId,
                CommentId = like.CommentId
            };
        }
    }
}
