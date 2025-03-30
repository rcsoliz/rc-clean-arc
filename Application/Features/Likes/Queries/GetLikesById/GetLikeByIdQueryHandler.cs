using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Likes.Queries.GetLikesById
{
    public class GetLikeByIdQueryHandler : IRequestHandler<GetLikeByIdQuery, LikeDto>
    {
        private readonly ILikeRepository _likeRepository;

        public GetLikeByIdQueryHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }
        public async Task<LikeDto> Handle(GetLikeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _likeRepository.GetLikeByIdAsync(request.Id);
        }
    }
}
