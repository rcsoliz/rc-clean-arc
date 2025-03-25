using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPostByUserId
{
    public record GetAllPostByUserIdQuery(int id) : IRequest<PagedResult<PostDto>>;


}
