using Application.DTOs;
using MediatR;

namespace Application.Features.Likes.Queries.GeAllLikes
{
    public record GetAllLikesQuery: IRequest<IEnumerable<LikeDto>>;

}
