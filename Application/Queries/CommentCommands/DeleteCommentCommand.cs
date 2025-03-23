using MediatR;

namespace Application.Queries.CommentCommands
{
    public record DeleteCommentCommand(int Id) : IRequest<bool>;
}
