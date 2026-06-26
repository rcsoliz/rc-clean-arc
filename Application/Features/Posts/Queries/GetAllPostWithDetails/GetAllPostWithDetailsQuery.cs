using Application.DTOs;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPostWithDetails
{
    public record GetAllPostWithDetailsQuery() : IRequest<IEnumerable<PostDto>>;
}
