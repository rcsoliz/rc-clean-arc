using Core.Entities;
using MediatR;

namespace Application.Queries.UserCommands
{
    public record CreateUserCommand(string Username, string Email,string Password) : IRequest<User>;
}
