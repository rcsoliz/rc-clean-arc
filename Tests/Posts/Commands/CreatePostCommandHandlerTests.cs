using Application.DTOs;
using Application.Features.Posts.Commands.CreatePost;
using Application.Features.PostsCategories.Commands.CreatePostCategories;
using Application.Features.PostsCategories.Commands.DeletePostCategories;
using Application.Features.PostsCategories.Commands.UpdatePostCategories;
using Application.Interfaces;
using Core.Entities;
using FluentAssertions;
using Moq;

namespace Tests.Posts.Commands
{
    public class CreatePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _repositoryMock;
        private readonly CreatePostCommandHandler _handler;

        public CreatePostCommandHandlerTests()
        {
            _repositoryMock = new Mock<IPostRepository>();
            _handler = new CreatePostCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsPostDto()
        {
            // Arrange
            var command = new CreatePostCommand("Contenido del post", 1, null, new List<int>());

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PostDto>();
            result.PostContent.Should().Be("Contenido del post");
            result.UserId.Should().Be(1);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsAddAsyncOnce()
        {
            // Arrange
            var command = new CreatePostCommand("Mi post", 2, "image.jpg", new List<int> { 1, 2 });

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<Post>(p => p.PostContent == "Mi post" && p.UserId == 2 && p.ImageUrl == "image.jpg"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithNullImageUrl_MapsImageUrlCorrectly()
        {
            // Arrange
            var command = new CreatePostCommand("Post sin imagen", 1, null, new List<int>());

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<Post>(p => p.ImageUrl == null),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    public class CreatePostCategoryCommandHandlerTests
    {
        private readonly Mock<IPostCategoryRepository> _repositoryMock;
        private readonly CreatePostCategoryCommandHandler _handler;

        public CreatePostCategoryCommandHandlerTests()
        {
            _repositoryMock = new Mock<IPostCategoryRepository>();
            _handler = new CreatePostCategoryCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsPostDto()
        {
            // Arrange
            var command = new CreatePostCategoryCommand("Contenido", 1, null, new List<int> { 1, 2 });

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.PostContent.Should().Be("Contenido");
            result.UserId.Should().Be(1);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsAddAsyncWithCorrectCategoryIds()
        {
            // Arrange
            var categoryIds = new List<int> { 1, 2, 3 };
            var command = new CreatePostCategoryCommand("Post con categorías", 1, null, categoryIds);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.IsAny<Post>(),
                    It.Is<List<int>>(ids => ids.SequenceEqual(categoryIds)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    public class UpdatePostCategoryCommandHandlerTests
    {
        private readonly Mock<IPostCategoryRepository> _repositoryMock;
        private readonly UpdatePostCategoryCommandHandler _handler;

        public UpdatePostCategoryCommandHandlerTests()
        {
            _repositoryMock = new Mock<IPostCategoryRepository>();
            _handler = new UpdatePostCategoryCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_PostExists_ReturnsTrue()
        {
            // Arrange
            var command = new UpdatePostCategoryCommand(1, "Nuevo contenido", 1, null, new List<int> { 1 });
            var existingPost = new PostDto { Id = 1, PostContent = "Contenido viejo" };

            _repositoryMock
                .Setup(r => r.GetPostByIdtWithCategoriesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingPost);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Post>(), It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_PostNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new UpdatePostCategoryCommand(99, "Contenido", 1, null, new List<int>());

            _repositoryMock
                .Setup(r => r.GetPostByIdtWithCategoriesAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PostDto?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_PostNotFound_NeverCallsUpdateAsync()
        {
            // Arrange
            var command = new UpdatePostCategoryCommand(99, "Contenido", 1, null, new List<int>());

            _repositoryMock
                .Setup(r => r.GetPostByIdtWithCategoriesAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PostDto?)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Post>(), It.IsAny<List<int>>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }

    public class DeletePostCategoryCommandHandlerTests
    {
        private readonly Mock<IPostCategoryRepository> _repositoryMock;
        private readonly DeletePostCategoryCommandHandler _handler;

        public DeletePostCategoryCommandHandlerTests()
        {
            _repositoryMock = new Mock<IPostCategoryRepository>();
            _handler = new DeletePostCategoryCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_PostExists_ReturnsTrue()
        {
            // Arrange
            var command = new DeletePostCategoryCommand(1, new List<int> { 1, 2 });
            var existingPost = new PostDto { Id = 1 };

            _repositoryMock
                .Setup(r => r.GetPostByIdtWithCategoriesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingPost);

            _repositoryMock
                .Setup(r => r.DeleteAsync(1, It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_PostNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new DeletePostCategoryCommand(99, new List<int> { 1 });

            _repositoryMock
                .Setup(r => r.GetPostByIdtWithCategoriesAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PostDto?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_PostNotFound_NeverCallsDeleteAsync()
        {
            // Arrange
            var command = new DeletePostCategoryCommand(99, new List<int> { 1 });

            _repositoryMock
                .Setup(r => r.GetPostByIdtWithCategoriesAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PostDto?)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<List<int>>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}