using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
              return await _likeRepository.GetAll();
        }
    }
}
