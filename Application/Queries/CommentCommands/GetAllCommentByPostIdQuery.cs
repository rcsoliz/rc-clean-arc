using Core.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CommentCommands
{
    public record GetAllCommentByPostIdQuery(int Id) : IRequest<IEnumerable<CommentDto>>;
}
