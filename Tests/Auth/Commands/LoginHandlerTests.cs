using Application.Features.Auth.Commands.Login;
using Application.Interfaces;
using Core.Entities;
using FluentAssertions;
using Moq;

namespace Tests.Auth.Commands
{
    public class LoginHandlerTests
    {
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            _jwtServiceMock = new Mock<IJwtService>();
            _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new LoginHandler(
                _jwtServiceMock.Object,
                _refreshTokenServiceMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var command = new LoginCommand { Email = "carlos@email.com", Password = "Password123" };
            var user = new User { Id = 1, Username = "carlos", Email = "carlos@email.com" };

            _userRepositoryMock
                .Setup(r => r.Access(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _jwtServiceMock
                .Setup(j => j.GenerateAccessToken(user))
                .Returns("access-token-jwt");

            _jwtServiceMock
                .Setup(j => j.GenerateRefreshToken())
                .Returns("refresh-token-abc");

            _refreshTokenServiceMock
                .Setup(r => r.SaveRefreshTokenAsync(user, "refresh-token-abc", It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.AccessToken.Should().Be("access-token-jwt");
            result.RefreshToken.Should().Be("refresh-token-abc");
        }

        [Fact]
        public async Task Handle_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var command = new LoginCommand { Email = "noexiste@email.com", Password = "WrongPassword" };

            _userRepositoryMock
                .Setup(r => r.Access(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_InvalidCredentials_NeverGeneratesTokens()
        {
            // Arrange
            var command = new LoginCommand { Email = "noexiste@email.com", Password = "WrongPassword" };

            _userRepositoryMock
                .Setup(r => r.Access(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _jwtServiceMock.Verify(j => j.GenerateAccessToken(It.IsAny<User>()), Times.Never);
            _jwtServiceMock.Verify(j => j.GenerateRefreshToken(), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidCredentials_SavesRefreshTokenInDb()
        {
            // Arrange
            var command = new LoginCommand { Email = "carlos@email.com", Password = "Password123" };
            var user = new User { Id = 1, Username = "carlos", Email = "carlos@email.com" };

            _userRepositoryMock
                .Setup(r => r.Access(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _jwtServiceMock.Setup(j => j.GenerateAccessToken(user)).Returns("access-token");
            _jwtServiceMock.Setup(j => j.GenerateRefreshToken()).Returns("refresh-token");

            _refreshTokenServiceMock
                .Setup(r => r.SaveRefreshTokenAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _refreshTokenServiceMock.Verify(
                r => r.SaveRefreshTokenAsync(user, "refresh-token", It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCredentials_CallsAccessWithCorrectEmail()
        {
            // Arrange
            var command = new LoginCommand { Email = "test@email.com", Password = "Password123" };

            _userRepositoryMock
                .Setup(r => r.Access(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(
                r => r.Access(
                    It.Is<User>(u => u.Email == "test@email.com"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}