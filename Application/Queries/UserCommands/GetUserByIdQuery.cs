using Core.Entities;
using MediatR;

namespace Application.Queries.UserCommands
{
    public record GetUserByIdQuery(int Id) : IRequest<User>;

}
