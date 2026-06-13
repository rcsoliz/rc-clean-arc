using Application.DTOs;
using Application.Features.Users.Commands.CreateUser;
using Application.Interfaces;
using Core.Entities;
using FluentAssertions;
using Moq;

namespace Tests.Users.Commands
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _handler = new CreateUserCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsUserDto()
        {
            // Arrange
            var command = new CreateUserCommand("carlos", "carlos@email.com", "Password123");
            var createdUser = new User { Id = 1, Username = "carlos", Email = "carlos@email.com" };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdUser);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserDto>();
            result!.Username.Should().Be("carlos");
            result.Email.Should().Be("carlos@email.com");
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsAddAsyncOnce()
        {
            // Arrange
            var command = new CreateUserCommand("ana", "ana@email.com", "Password123");
            var createdUser = new User { Id = 2, Username = "ana", Email = "ana@email.com" };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdUser);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<User>(u => u.Username == "ana" && u.Email == "ana@email.com"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_PasswordIsHashedNotPlainText()
        {
            // Arrange
            var command = new CreateUserCommand("pedro", "pedro@email.com", "MiPassword123");
            var createdUser = new User { Id = 3, Username = "pedro", Email = "pedro@email.com" };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdUser);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert — verificar que la contraseña NO se guarda en texto plano
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<User>(u => u.PasswordHash != "MiPassword123"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnedDtoHasCorrectId()
        {
            // Arrange
            var command = new CreateUserCommand("luis", "luis@email.com", "Password123");
            var createdUser = new User { Id = 5, Username = "luis", Email = "luis@email.com" };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdUser);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result!.Id.Should().Be(5);
        }
    }
}