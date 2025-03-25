using Core.Entities;
using MediatR;

namespace Application.Features.Comments.Queries.GetAllComments
{
    public record GetAllCommentQuery: IRequest<IEnumerable<Comment>>;
}
