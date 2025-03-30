using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Likes.Commands.UpdateLike
{
    public class UpdateLikeCommandHandler : IRequestHandler<UpdateLikeCommand, bool>
    {
        private readonly ILikeRepository _likeRepository;

        public  UpdateLikeCommandHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<bool> Handle(UpdateLikeCommand request, CancellationToken cancellationToken)
        {
            var like = await _likeRepository.GetLikeByIdAsync(request.Id);
            if (like == null) return false;
            var likeItem = new Like
            {
                Id = request.Id,
                UserId = request.UserId,
                PostId = request.PostId,
                CommentId = request.CommentId,
            };
           
            await _likeRepository.UpdateAsync(likeItem);
            return true;
        }
    }
}
