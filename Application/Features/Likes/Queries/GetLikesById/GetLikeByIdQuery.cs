using Application.DTOs;
using MediatR;

namespace Application.Features.Likes.Queries.GetLikesById
{
    public record GetLikeByIdQuery(int Id) : IRequest<LikeDto>;
    
}
