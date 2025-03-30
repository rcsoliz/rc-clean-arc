using MediatR;

namespace Application.Features.Likes.Commands.UpdateLike
{
    public record UpdateLikeCommand(int Id, int UserId, int PostId, int CommentId) : IRequest<bool>;

}
