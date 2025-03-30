using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Likes.Commands.CreateLike
{
    public class CreateLikeCommandHandler : IRequestHandler<CreateLikeCommand, Like>
    {
        private readonly ILikeRepository _likeRepository;

        public CreateLikeCommandHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<Like> Handle(CreateLikeCommand request, CancellationToken cancellationToken)
        {
            var like = new Like
            {
                UserId = request.UserId,
                PostId = request.PostId,
                CommentId = request.CommentId,
            };

           await _likeRepository.AddAsync(like);

            return like;
        }
    }
}
