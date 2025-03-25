using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Features.Posts.Queries.FiltersPos
{
    public record FiltersPostQuery (PostFilterDto filter) :  IRequest<PagedResult<PostDto>>;
}
