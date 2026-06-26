using Application.DTOs;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(int Id, string Username, string? Bio, string? AvatarUrl) : IRequest<UserDto?>;
}
