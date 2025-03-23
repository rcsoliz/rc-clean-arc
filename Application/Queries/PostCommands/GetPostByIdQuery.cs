using Core.Dtos;
using Core.Entities;
using MediatR;

namespace Application.Queries.PostCommands
{
    public record GetPostByIdQuery(int id) : IRequest<PostDto>;

}
