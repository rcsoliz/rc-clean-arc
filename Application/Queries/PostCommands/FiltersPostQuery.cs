using Application.Common;
using Core.Dtos;
using MediatR;

namespace Application.Queries.PostCommands
{
    public record FiltersPostQuery (PostFilterDto filter) :  IRequest<PagedResult<PostDto>>;
}
