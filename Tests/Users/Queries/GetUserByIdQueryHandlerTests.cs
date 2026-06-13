using Application.DTOs;
using Application.Features.Users.Queries.GetUserById;
using Application.Interfaces;
using Core.Entities;
using FluentAssertions;
using Moq;

namespace Tests.Users.Queries
{
    public class GetUserByIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _handler = new GetUserByIdQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_UserExists_ReturnsUserDto()
        {
            // Arrange
            var user = new User { Id = 1, Username = "carlos", Email = "carlos@email.com" };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(new GetUserByIdQuery(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserDto>();
            result!.Id.Should().Be(1);
            result.Username.Should().Be("carlos");
            result.Email.Should().Be("carlos@email.com");
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _handler.Handle(new GetUserByIdQuery(99), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_UserExists_MapsAllFieldsCorrectly()
        {
            // Arrange
            var user = new User { Id = 3, Username = "ana", Email = "ana@email.com" };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(new GetUserByIdQuery(3), CancellationToken.None);

            // Assert
            result!.Id.Should().Be(user.Id);
            result.Username.Should().Be(user.Username);
            result.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task Handle_Always_CallsGetByIdAsyncWithCorrectId()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(7, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            await _handler.Handle(new GetUserByIdQuery(7), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.GetByIdAsync(7, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}