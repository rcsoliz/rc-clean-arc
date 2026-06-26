using Application.DTOs;
using Application.Features.Likes.Commands.CreateLike;
using Application.Features.Likes.Commands.DeleteLike;
using Application.Features.Likes.Commands.UpdateLike;
using Application.Interfaces;
using Core.Entities;
using FluentAssertions;
using Moq;

namespace Tests.Likes.Commands
{
    public class CreateLikeCommandHandlerTests
    {
        private readonly Mock<ILikeRepository> _repositoryMock;
        private readonly CreateLikeCommandHandler _handler;

        public CreateLikeCommandHandlerTests()
        {
            _repositoryMock = new Mock<ILikeRepository>();
            var postRepositoryMock = new Mock<IPostRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            postRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PostDto?)null);

            _handler = new CreateLikeCommandHandler(
                _repositoryMock.Object,
                postRepositoryMock.Object,
                notificationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsLikeDto()
        {
            // Arrange
            var command = new CreateLikeCommand(1, 10, null);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Like>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<LikeDto>();
            result!.UserId.Should().Be(1);
            result.PostId.Should().Be(10);
            result.CommentId.Should().BeNull();
        }

        [Fact]
        public async Task Handle_WithCommentId_MapsCommentIdCorrectly()
        {
            // Arrange
            var command = new CreateLikeCommand(1, 10, 5);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Like>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result!.CommentId.Should().Be(5);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsAddAsyncOnce()
        {
            // Arrange
            var command = new CreateLikeCommand(2, 3, null);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Like>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<Like>(l => l.UserId == 2 && l.PostId == 3),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    public class UpdateLikeCommandHandlerTests
    {
        private readonly Mock<ILikeRepository> _repositoryMock;
        private readonly UpdateLikeCommandHandler _handler;

        public UpdateLikeCommandHandlerTests()
        {
            _repositoryMock = new Mock<ILikeRepository>();
            _handler = new UpdateLikeCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_LikeExists_ReturnsTrue()
        {
            // Arrange
            var command = new UpdateLikeCommand(1, 2, 3, 4);
            var existingLike = new Like { Id = 1, UserId = 1, PostId = 1, CommentId = 1 };

            _repositoryMock
                .Setup(r => r.GetLikeByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingLike);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Like>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_LikeNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateLikeCommand(99, 1, 1, 1);

            _repositoryMock
                .Setup(r => r.GetLikeByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Like?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_LikeNotFound_NeverCallsUpdateAsync()
        {
            // Arrange
            var command = new UpdateLikeCommand(99, 1, 1, 1);

            _repositoryMock
                .Setup(r => r.GetLikeByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Like?)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Like>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_LikeExists_CallsUpdateAsyncWithCorrectValues()
        {
            // Arrange
            var command = new UpdateLikeCommand(1, 2, 3, 4);
            var existingLike = new Like { Id = 1, UserId = 1, PostId = 1, CommentId = 1 };

            _repositoryMock
                .Setup(r => r.GetLikeByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingLike);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Like>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.UpdateAsync(
                    It.Is<Like>(l => l.Id == 1 && l.UserId == 2 && l.PostId == 3 && l.CommentId == 4),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    public class DeleteLikeCommandHandlerTests
    {
        private readonly Mock<ILikeRepository> _repositoryMock;
        private readonly DeleteLikeCommandHandler _handler;

        public DeleteLikeCommandHandlerTests()
        {
            _repositoryMock = new Mock<ILikeRepository>();
            _handler = new DeleteLikeCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_LikeExists_ReturnsTrue()
        {
            // Arrange
            var command = new DeleteLikeCommand(1);

            _repositoryMock
                .Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_LikeNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new DeleteLikeCommand(99);

            _repositoryMock
                .Setup(r => r.DeleteAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_AnyCommand_CallsDeleteAsyncWithCorrectId()
        {
            // Arrange
            var command = new DeleteLikeCommand(5);

            _repositoryMock
                .Setup(r => r.DeleteAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.DeleteAsync(5, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}