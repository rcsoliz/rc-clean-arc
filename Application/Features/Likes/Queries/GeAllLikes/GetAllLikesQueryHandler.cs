using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Likes.Queries.GeAllLikes
{
    public class GetAllLikesQueryHandler : IRequestHandler<GetAllLikesQuery, IEnumerable<LikeDto>>
    {
        private readonly ILikeRepository _likeRepository;

        public GetAllLikesQueryHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<IEnumerable<LikeDto>> Handle(GetAllLikesQuery request, CancellationToken cancellationToken)
        {
            return await _likeRepository.GetAll(cancellationToken);
        }
    }
}
