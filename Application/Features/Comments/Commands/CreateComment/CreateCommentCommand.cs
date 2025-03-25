using Core.Entities;
using Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comments.Commands.CreateComment
{
    public record CreateCommentCommand(string CommentContent, int UserId, int PostId, int? ParentCommentId) : IRequest<CommentModel>;

}
