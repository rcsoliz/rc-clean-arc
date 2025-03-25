using Application.Common;
using Core.Dtos;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPostByUserId
{
    public record GetAllPostByUserIdQuery(int id) : IRequest<PagedResult<PostDto>>;


}
