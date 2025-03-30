using MediatR;

namespace Application.Features.Likes.Commands.DeleteLike
{
    public record DeleteLikeCommand(int Id) : IRequest<bool>;
    
}
