using Application.DTOs;
using Application.Features.Comments.Commands.CreateComment;
using Application.Interfaces;
using Core.Entities;
using FluentAssertions;
using Moq;

namespace Tests.Comments.Commands
{
    public class CreateCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _repositoryMock;
        private readonly CreateCommentCommandHandler _handler;

        public CreateCommentCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICommentRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var notificationServiceMock = new Mock<INotificationService>();
            var postRepositoryMock = new Mock<IPostRepository>();

            userRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            postRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PostDto?)null);

            _handler = new CreateCommentCommandHandler(
                _repositoryMock.Object,
                userRepositoryMock.Object,
                notificationServiceMock.Object,
                postRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsCommentDto()
        {
            // Arrange
            var command = new CreateCommentCommand("Gran artículo!", 1, 10, null);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Comment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CommentDto>();
            result.CommentContent.Should().Be("Gran artículo!");
            result.UserId.Should().Be(1);
            result.PostId.Should().Be(10);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsAddAsyncOnce()
        {
            // Arrange
            var command = new CreateCommentCommand("Excelente post", 2, 5, null);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Comment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<Comment>(c =>
                        c.CommentContent == "Excelente post" &&
                        c.UserId == 2 &&
                        c.PostId == 5),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithParentCommentId_MapsParentCommentIdCorrectly()
        {
            // Arrange
            var command = new CreateCommentCommand("Respuesta al comentario", 1, 10, 3);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Comment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<Comment>(c => c.ParentCommentId == 3),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithoutParentCommentId_ParentCommentIdIsNull()
        {
            // Arrange
            var command = new CreateCommentCommand("Comentario raíz", 1, 10, null);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Comment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<Comment>(c => c.ParentCommentId == null),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}