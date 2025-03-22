using Core.Entities;
using MediatR;

namespace Application.Queries.PostCommands
{
    public record GetPostByIdQuery(int id) : IRequest<Post>;

}
