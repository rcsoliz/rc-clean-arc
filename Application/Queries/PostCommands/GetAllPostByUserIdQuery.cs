using Application.Common;
using Core.Dtos;
using MediatR;

namespace Application.Queries.PostCommands
{
    public record GetAllPostByUserIdQuery(int id) : IRequest<PagedResult<PostDto>>;


}
