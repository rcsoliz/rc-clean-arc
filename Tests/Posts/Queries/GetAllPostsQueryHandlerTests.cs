using Application.Common;
using Application.DTOs;
using Application.Features.Posts.Queries.FiltersPos;
using Application.Features.Posts.Queries.GetAllPost;
using Application.Features.Posts.Queries.GetAllPostByUserId;
using Application.Features.Posts.Queries.GetAllPosts;
using Application.Features.Posts.Queries.GetPostById;
using Application.Interfaces;
using Core.Entities;
using FluentAssertions;
using Moq;

namespace Tests.Posts.Queries
{
    public class GetAllPostsQueryHandlerTests
    {
        private readonly Mock<IPostRepository> _repositoryMock;
        private readonly GetAllPostsQueryHandlers _handler;

        public GetAllPostsQueryHandlerTests()
        {
            _repositoryMock = new Mock<IPostRepository>();
            _handler = new GetAllPostsQueryHandlers(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_PostsExist_ReturnsAllPostsAsDtos()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1, PostContent = "Post 1", UserId = 1, User = new User { Username = "carlos" } },
                new Post { Id = 2, PostContent = "Post 2", UserId = 2, User = new User { Username = "ana" } }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(posts);

            // Act
            var result = await _handler.Handle(new GetAllPostQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_PostsExist_MapsUsernameCorrectly()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1, PostContent = "Post", UserId = 1, User = new User { Username = "carlos" } }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(posts);

            // Act
            var result = await _handler.Handle(new GetAllPostQuery(), CancellationToken.None);

            // Assert
            result.First().Username.Should().Be("carlos");
        }

        [Fact]
        public async Task Handle_PostWithNullUser_UsernameIsEmptyString()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1, PostContent = "Post", UserId = 1, User = null! }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(posts);

            // Act
            var result = await _handler.Handle(new GetAllPostQuery(), CancellationToken.None);

            // Assert
            result.First().Username.Should().Be(string.Empty);
        }

        [Fact]
        public async Task Handle_NoPosts_ReturnsEmptyList()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Post>());

            // Act
            var result = await _handler.Handle(new GetAllPostQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }

    public class GetPostByIdQueryHandlerTests
    {
        private readonly Mock<IPostRepository> _repositoryMock;
        private readonly GetPostByIdQueryHandler _handler;

        public GetPostByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IPostRepository>();
            _handler = new GetPostByIdQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_PostExists_ReturnsPostDto()
        {
            // Arrange
            var dto = new PostDto { Id = 1, PostContent = "Mi post", UserId = 1 };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            // Act
            var result = await _handler.Handle(new GetPostByIdQuery(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.PostContent.Should().Be("Mi post");
        }

        [Fact]
        public async Task Handle_PostNotFound_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PostDto?)null);

            // Act
            var result = await _handler.Handle(new GetPostByIdQuery(99), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Always_CallsGetByIdAsyncWithCorrectId()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PostDto?)null);

            // Act
            await _handler.Handle(new GetPostByIdQuery(5), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    public class GetAllPostByUserIdQueryHandlerTests
    {
        private readonly Mock<IPostRepository> _repositoryMock;
        private readonly GetAllPostByUserIdQueryHandler _handler;

        public GetAllPostByUserIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IPostRepository>();
            _handler = new GetAllPostByUserIdQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_PostsExistForUser_ReturnsPagedResult()
        {
            // Arrange
            var pagedResult = new PagedResult<PostDto>
            {
                Items = new List<PostDto>
                {
                    new PostDto { Id = 1, PostContent = "Post 1", UserId = 1 },
                    new PostDto { Id = 2, PostContent = "Post 2", UserId = 1 }
                },
                TotalCount = 2
            };

            _repositoryMock
                .Setup(r => r.GetAllPostByUserId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _handler.Handle(new GetAllPostByUserIdQuery(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task Handle_NoPostsForUser_ReturnsEmptyPagedResult()
        {
            // Arrange
            var emptyResult = new PagedResult<PostDto> { Items = new List<PostDto>(), TotalCount = 0 };

            _repositoryMock
                .Setup(r => r.GetAllPostByUserId(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyResult);

            // Act
            var result = await _handler.Handle(new GetAllPostByUserIdQuery(99), CancellationToken.None);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }
    }

    public class GetPagedPostsAsyncRefactoryHandlerTests
    {
        private readonly Mock<IPostRepository> _repositoryMock;
        private readonly GetPagedPostsAsyncRefactoryHandler _handler;

        public GetPagedPostsAsyncRefactoryHandlerTests()
        {
            _repositoryMock = new Mock<IPostRepository>();
            _handler = new GetPagedPostsAsyncRefactoryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidPageParams_ReturnsPagedResult()
        {
            // Arrange
            var pagedResult = new PagedResult<PostDto>
            {
                Items = new List<PostDto>
                {
                    new PostDto { Id = 1, PostContent = "Post 1" },
                    new PostDto { Id = 2, PostContent = "Post 2" }
                },
                TotalCount = 10
            };

            _repositoryMock
                .Setup(r => r.GetPagedPostsAsyncRefactory(1, 5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            // Act
            var query = new GetPagedPostsAsyncRefactoryQuery(1, 5);
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(10);
        }

        [Fact]
        public async Task Handle_Always_CallsRepositoryWithCorrectPageParams()
        {
            // Arrange
            var emptyResult = new PagedResult<PostDto> { Items = new List<PostDto>(), TotalCount = 0 };

            _repositoryMock
                .Setup(r => r.GetPagedPostsAsyncRefactory(2, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyResult);

            // Act
            var query = new GetPagedPostsAsyncRefactoryQuery(2, 10);
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.GetPagedPostsAsyncRefactory(2, 10, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    public class FiltersPostQueryHandlerTests
    {
        private readonly Mock<IFilterRepository> _repositoryMock;
        private readonly FiltersPostQueryHandler _handler;

        public FiltersPostQueryHandlerTests()
        {
            _repositoryMock = new Mock<IFilterRepository>();
            _handler = new FiltersPostQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WithSearchText_ReturnsFilteredResults()
        {
            // Arrange
            var filter = new PostFilterDto { SearchText = "tecnología", Page = 1, PageSize = 10 };
            var pagedResult = new PagedResult<PostDto>
            {
                Items = new List<PostDto> { new PostDto { Id = 1, PostContent = "Post de tecnología" } },
                TotalCount = 1
            };

            _repositoryMock
                .Setup(r => r.FiltersPost(It.IsAny<PostFilterDto>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _handler.Handle(new FiltersPostQuery(filter), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(1);
        }

        [Fact]
        public async Task Handle_NoMatchingPosts_ReturnsEmptyPagedResult()
        {
            // Arrange
            var filter = new PostFilterDto { SearchText = "xyz_no_existe", Page = 1, PageSize = 10 };
            var emptyResult = new PagedResult<PostDto> { Items = new List<PostDto>(), TotalCount = 0 };

            _repositoryMock
                .Setup(r => r.FiltersPost(It.IsAny<PostFilterDto>()))
                .ReturnsAsync(emptyResult);

            // Act
            var result = await _handler.Handle(new FiltersPostQuery(filter), CancellationToken.None);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task Handle_Always_CallsFiltersPostWithCorrectFilter()
        {
            // Arrange
            var filter = new PostFilterDto { Username = "carlos", Page = 1, PageSize = 5 };
            var emptyResult = new PagedResult<PostDto> { Items = new List<PostDto>(), TotalCount = 0 };

            _repositoryMock
                .Setup(r => r.FiltersPost(It.IsAny<PostFilterDto>()))
                .ReturnsAsync(emptyResult);

            // Act
            await _handler.Handle(new FiltersPostQuery(filter), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.FiltersPost(It.Is<PostFilterDto>(f => f.Username == "carlos")),
                Times.Once);
        }
    }
}