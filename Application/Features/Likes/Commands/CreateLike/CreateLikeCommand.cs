using Core.Entities;
using MediatR;

namespace Application.Features.Likes.Commands.CreateLike
{
    public record CreateLikeCommand (int UserId, int PostId, int CommentId ) : IRequest<Like>;
    
}
