using Core.Entities;
using MediatR;

namespace Application.Queries.PostCommands
{
    public record GetAllPostQuery(): IRequest<IEnumerable<Post>>;
}
