using Application.DTOs;
using Application.Features.Categories.Queries.GetAllCategories;
using Application.Interfaces;
using CategoryEntity = Core.Entities.Category;
using FluentAssertions;
using Moq;

namespace Tests.Category.Queries
{
    public class GetAllCategoriesQueryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly GetAllCategoriesQueryHandlers _handler;

        public GetAllCategoriesQueryHandlerTests()
        {
            _repositoryMock = new Mock<ICategoryRepository>();
            _handler = new GetAllCategoriesQueryHandlers(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_CategoriesExist_ReturnsAllCategoriesAsDtos()
        {
            // Arrange
            var categories = new List<CategoryEntity>
            {
                new CategoryEntity { Id = 1, Name = "Tecnología" },
                new CategoryEntity    { Id = 2, Name = "Deportes" },
                new CategoryEntity { Id = 3, Name = "Ciencia" }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeOfType<List<CategoryDto>>();
        }

        [Fact]
        public async Task Handle_CategoriesExist_MapsNamesCorrectly()
        {
            // Arrange
            var categories = new List<CategoryEntity>
            {
                new CategoryEntity { Id = 1, Name = "Tecnología" },
                new CategoryEntity { Id = 2, Name = "Deportes" }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

            // Assert
            result.Select(c => c.Name).Should().BeEquivalentTo("Tecnología", "Deportes");
        }

        [Fact]
        public async Task Handle_NoCategoriesExist_ReturnsEmptyList()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CategoryEntity>());

            // Act
            var result = await _handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

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
                .ReturnsAsync(new List<CategoryEntity>());

            // Act
            await _handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}