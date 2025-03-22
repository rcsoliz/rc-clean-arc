using Core.Entities;
using MediatR;

namespace Application.Queries.CommentCommands
{
    public record GetAllCommentQuery: IRequest<IEnumerable<Comment>>;
}
