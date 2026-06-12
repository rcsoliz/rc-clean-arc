using Application.Interfaces;
using MediatR;

namespace Application.Features.Likes.Commands.DeleteLike
{
    public class DeleteLikeCommandHandler : IRequestHandler<DeleteLikeCommand, bool>
    {
        private readonly ILikeRepository _likeRepository;
        public DeleteLikeCommandHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<bool> Handle(DeleteLikeCommand request, CancellationToken cancellationToken)
        {
            return await _likeRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
