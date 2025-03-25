using Core.Entities;
using MediatR;

namespace Application.Features.Posts.Queries.GetAllPost
{
    public record GetAllPostQuery(): IRequest<IEnumerable<Post>>;
}
