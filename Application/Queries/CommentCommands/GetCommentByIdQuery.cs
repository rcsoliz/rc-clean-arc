using Core.Entities;
using MediatR;


namespace Application.Queries.CommentCommands
{
    public record GetCommentByIdQuery(int Id) : IRequest<Comment>;

}
