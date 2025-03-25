using MediatR;

namespace Application.Features.Comments.Commands.DeleteComment
{
    public record DeleteCommentCommand(int Id) : IRequest<bool>;
}
