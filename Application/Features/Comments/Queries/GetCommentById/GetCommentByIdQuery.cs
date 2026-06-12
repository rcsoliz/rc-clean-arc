using Application.DTOs;
using MediatR;


namespace Application.Features.Comments.Queries.GetCommentById
{
    public record GetCommentByIdQuery(int Id) : IRequest<CommentDto>;

}
