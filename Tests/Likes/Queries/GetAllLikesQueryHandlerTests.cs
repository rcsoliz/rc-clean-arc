using Application.DTOs;
using Application.Features.Likes.Queries.GetAllLikes;
using Application.Features.Likes.Queries.GetLikesById;
using Application.Interfaces;
using Core.Entities;
using FluentAssertions;
using Moq;

namespace Tests.Likes.Queries
{
    public class GetAllLikesQueryHandlerTests
    {
        private readonly Mock<ILikeRepository> _repositoryMock;
        private readonly GetAllLikesQueryHandler _handler;

        public GetAllLikesQueryHandlerTests()
        {
            _repositoryMock = new Mock<ILikeRepository>();
            _handler = new GetAllLikesQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_LikesExist_ReturnsAllLikesAsDtos()
        {
            // Arrange
            var dtos = new List<LikeDto>
            {
                new LikeDto { Id = 1, UserId = 1, PostId = 1 },
                new LikeDto { Id = 2, UserId = 2, PostId = 1 },
                new LikeDto { Id = 3, UserId = 1, PostId = 2 }
            };

            _repositoryMock
                .Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<LikeDto>)dtos);

            // Act
            var result = await _handler.Handle(new GetAllLikesQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task Handle_NoLikes_ReturnsEmptyList()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<LikeDto>)new List<LikeDto>());

            // Act
            var result = await _handler.Handle(new GetAllLikesQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Always_CallsGetAllOnce()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<LikeDto>)new List<LikeDto>());

            // Act
            await _handler.Handle(new GetAllLikesQuery(), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.GetAll(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_LikesExist_DelegatesDirectlyToRepository()
        {
            // Arrange — el handler no hace mapeo, delega al repositorio
            var dtos = new List<LikeDto>
            {
                new LikeDto { Id = 1, UserId = 5, PostId = 3, CommentId = 2 }
            };

            _repositoryMock
                .Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<LikeDto>)dtos);

            // Act
            var result = await _handler.Handle(new GetAllLikesQuery(), CancellationToken.None);

            // Assert
            var like = result.First();
            like.UserId.Should().Be(5);
            like.PostId.Should().Be(3);
            like.CommentId.Should().Be(2);
        }
    }

    public class GetLikeByIdQueryHandlerTests
    {
        private readonly Mock<ILikeRepository> _repositoryMock;
        private readonly GetLikeByIdQueryHandler _handler;

        public GetLikeByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<ILikeRepository>();
            _handler = new GetLikeByIdQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_LikeExists_ReturnsLikeDto()
        {
            // Arrange
            var like = new Like { Id = 1, UserId = 1, PostId = 5, CommentId = null };

            _repositoryMock
                .Setup(r => r.GetLikeByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(like);

            // Act
            var result = await _handler.Handle(new GetLikeByIdQuery(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.UserId.Should().Be(1);
            result.PostId.Should().Be(5);
            result.CommentId.Should().BeNull();
        }

        [Fact]
        public async Task Handle_LikeNotFound_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetLikeByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Like?)null);

            // Act
            var result = await _handler.Handle(new GetLikeByIdQuery(99), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_LikeWithCommentId_MapsCommentIdCorrectly()
        {
            // Arrange
            var like = new Like { Id = 1, UserId = 1, PostId = 5, CommentId = 3 };

            _repositoryMock
                .Setup(r => r.GetLikeByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(like);

            // Act
            var result = await _handler.Handle(new GetLikeByIdQuery(1), CancellationToken.None);

            // Assert
            result!.CommentId.Should().Be(3);
        }

        [Fact]
        public async Task Handle_Always_CallsGetLikeByIdAsyncWithCorrectId()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetLikeByIdAsync(7, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Like?)null);

            // Act
            await _handler.Handle(new GetLikeByIdQuery(7), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.GetLikeByIdAsync(7, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}