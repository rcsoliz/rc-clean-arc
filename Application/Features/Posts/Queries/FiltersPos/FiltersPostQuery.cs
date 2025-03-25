using Application.Common;
using Core.Dtos;
using MediatR;

namespace Application.Features.Posts.Queries.FiltersPos
{
    public record FiltersPostQuery (PostFilterDto filter) :  IRequest<PagedResult<PostDto>>;
}
