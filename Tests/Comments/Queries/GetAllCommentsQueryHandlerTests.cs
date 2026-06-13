using Application.DTOs;
using Application.Features.Comments.Queries.GetAllCommentByPostId;
using Application.Features.Comments.Queries.GetAllComments;
using Application.Features.Comments.Queries.GetCommentById;
using Application.Interfaces;
using Core.Entities;
using FluentAssertions;
using Moq;

namespace Tests.Comments.Queries
{
    public class GetAllCommentsQueryHandlerTests
    {
        private readonly Mock<ICommentRepository> _repositoryMock;
        private readonly GetAllCommentsQueryHandlers _handler;

        public GetAllCommentsQueryHandlerTests()
        {
            _repositoryMock = new Mock<ICommentRepository>();
            _handler = new GetAllCommentsQueryHandlers(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_CommentsExist_ReturnsAllCommentsAsDtos()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, CommentContent = "Primer comentario", UserId = 1, PostId = 1,
                    User = new User { Username = "usuario1" } },
                new Comment { Id = 2, CommentContent = "Segundo comentario", UserId = 2, PostId = 1,
                    User = new User { Username = "usuario2" } }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(comments);

            // Act
            var result = await _handler.Handle(new GetAllCommentQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_CommentsExist_MapsContentCorrectly()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, CommentContent = "Hola mundo", UserId = 1, PostId = 1,
                    User = new User { Username = "carlos" } }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(comments);

            // Act
            var result = await _handler.Handle(new GetAllCommentQuery(), CancellationToken.None);

            // Assert
            var comment = result.First();
            comment.CommentContent.Should().Be("Hola mundo");
            comment.UserId.Should().Be(1);
            comment.PostId.Should().Be(1);
        }

        [Fact]
        public async Task Handle_NoComments_ReturnsEmptyList()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Comment>());

            // Act
            var result = await _handler.Handle(new GetAllCommentQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Always_CallsGetAllAsyncOnce()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Comment>());

            // Act
            await _handler.Handle(new GetAllCommentQuery(), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    public class GetCommentByIdQueryHandlerTests
    {
        private readonly Mock<ICommentRepository> _repositoryMock;
        private readonly GetCommentByIdQueryHandler _handler;

        public GetCommentByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<ICommentRepository>();
            _handler = new GetCommentByIdQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_CommentExists_ReturnsCommentDto()
        {
            // Arrange
            var comment = new Comment
            {
                Id = 1,
                CommentContent = "Test comment",
                UserId = 1,
                PostId = 2,
                User = new User { Username = "usuario1" }
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(comment);

            // Act
            var result = await _handler.Handle(new GetCommentByIdQuery(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.CommentContent.Should().Be("Test comment");
            result.UserId.Should().Be(1);
            result.PostId.Should().Be(2);
        }

        [Fact]
        public async Task Handle_CommentNotFound_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Comment?)null);

            // Act
            var result = await _handler.Handle(new GetCommentByIdQuery(99), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Always_CallsGetByIdAsyncWithCorrectId()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Comment?)null);

            // Act
            await _handler.Handle(new GetCommentByIdQuery(5), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    public class GetAllCommentByPostIdQueryHandlerTests
    {
        private readonly Mock<ICommentRepository> _repositoryMock;
        private readonly GetAllCommentByPostIdQueryHandlers _handler;

        public GetAllCommentByPostIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<ICommentRepository>();
            _handler = new GetAllCommentByPostIdQueryHandlers(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_CommentsExistForPost_ReturnsDtos()
        {
            // Arrange
            var dtos = new List<CommentDto>
            {
                new CommentDto { Id = 1, CommentContent = "Primer comentario", UserId = 1, PostId = 5 },
                new CommentDto { Id = 2, CommentContent = "Segundo comentario", UserId = 2, PostId = 5 }
            };

            _repositoryMock
                .Setup(r => r.GetAllCommentByPostId(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<CommentDto>)dtos);

            // Act
            var result = await _handler.Handle(new GetAllCommentByPostIdQuery(5), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.All(c => c.PostId == 5).Should().BeTrue();
        }

        [Fact]
        public async Task Handle_NoCommentsForPost_ReturnsEmptyList()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAllCommentByPostId(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<CommentDto>)new List<CommentDto>());

            // Act
            var result = await _handler.Handle(new GetAllCommentByPostIdQuery(99), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Always_CallsRepositoryWithCorrectPostId()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAllCommentByPostId(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<CommentDto>)new List<CommentDto>());

            // Act
            await _handler.Handle(new GetAllCommentByPostIdQuery(3), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.GetAllCommentByPostId(3, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}