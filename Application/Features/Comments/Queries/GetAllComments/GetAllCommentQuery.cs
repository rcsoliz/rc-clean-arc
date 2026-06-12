using Application.DTOs;
using MediatR;

namespace Application.Features.Comments.Queries.GetAllComments
{
    public record GetAllCommentQuery: IRequest<IEnumerable<CommentDto>>;
}
