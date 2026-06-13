using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto?>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
               
            };
            user.SetPassword(request.Password);

            var createdUser = await _userRepository.AddAsync(user, cancellationToken);

            return new UserDto
            {
                Id = createdUser.Id,
                Username = createdUser.Username,
                Email = createdUser.Email
            };
        }
    }

}
