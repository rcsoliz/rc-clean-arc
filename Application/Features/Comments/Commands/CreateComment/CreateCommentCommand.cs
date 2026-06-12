using Application.DTOs;
using MediatR;

namespace Application.Features.Comments.Commands.CreateComment
{
    public record CreateCommentCommand(string CommentContent, int UserId, int PostId, int? ParentCommentId) 
        : IRequest<CommentDto>;

}
