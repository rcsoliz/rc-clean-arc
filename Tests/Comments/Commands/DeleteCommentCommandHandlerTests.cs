using Application.Features.Comments.Commands.DeleteComment;
using Application.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.Comments.Commands
{
    public class DeleteCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _repositoryMock;
        private readonly DeleteCommentCommandHandler _handler;

        public DeleteCommentCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICommentRepository>();
            _handler = new DeleteCommentCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_CommentExists_ReturnsTrue()
        {
            // Arrange
            var command = new DeleteCommentCommand(1);

            _repositoryMock
                .Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_CommentNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new DeleteCommentCommand(99);

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
            var command = new DeleteCommentCommand(7);

            _repositoryMock
                .Setup(r => r.DeleteAsync(7, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.DeleteAsync(7, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}