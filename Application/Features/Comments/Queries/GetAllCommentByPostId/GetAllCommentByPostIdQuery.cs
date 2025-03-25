using Application.DTOs;
using MediatR;

namespace Application.Features.Comments.Queries.GetAllCommentByPostId
{
    public record GetAllCommentByPostIdQuery(int Id) : IRequest<IEnumerable<CommentDto>>;
}
