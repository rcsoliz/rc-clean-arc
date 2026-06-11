using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Likes.Commands.CreateLike
{
    public class CreateLikeCommandHandler : IRequestHandler<CreateLikeCommand, LikeDto>
    {
        private readonly ILikeRepository _likeRepository;

        public CreateLikeCommandHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<LikeDto> Handle(CreateLikeCommand request, CancellationToken cancellationToken)
        {
            var like = new LikeDto
            {
                UserId = request.UserId,
                PostId = request.PostId,
                CommentId = request.CommentId,
            };

            var createdLike = new Like
            {
                UserId = like.UserId,
                PostId = like.PostId,
                CommentId = like.CommentId,
            };

            await _likeRepository.AddAsync(createdLike);

            return like;
        }
    }
}
