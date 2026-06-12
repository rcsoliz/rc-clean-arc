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
            var likes = await _likeRepository.GetAll(cancellationToken);    
            return likes.Select(l => new LikeDto
            {
                Id = l.Id,
                PostId = l.PostId,
                UserId = l.UserId
            });
        }
    }
}
