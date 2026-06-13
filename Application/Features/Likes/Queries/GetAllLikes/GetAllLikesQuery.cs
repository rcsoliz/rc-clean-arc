using Application.DTOs;
using MediatR;

namespace Application.Features.Likes.Queries.GetAllLikes
{
    public record GetAllLikesQuery: IRequest<IEnumerable<LikeDto>>;

}
