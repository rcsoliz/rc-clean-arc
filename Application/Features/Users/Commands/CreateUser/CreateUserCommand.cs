using Core.Entities;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand(string Username, string Email,string Password) : IRequest<User>;
}
