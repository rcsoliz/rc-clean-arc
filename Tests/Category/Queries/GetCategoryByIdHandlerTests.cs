using Application.DTOs;
using Application.Features.Categories.Queries.GetCategoryById;
using Application.Interfaces;
using CategoryEntity = Core.Entities.Category;
using FluentAssertions;
using Moq;

namespace Tests.Category.Queries
{
    public class GetCategoryByIdHandlerTests
    {
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly GetCategoryByIdHandlers _handler;

        public GetCategoryByIdHandlerTests()
        {
            _repositoryMock = new Mock<ICategoryRepository>();
            _handler = new GetCategoryByIdHandlers(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_CategoryExists_ReturnsCategoryDto()
        {
            // Arrange
            var category = new CategoryEntity { Id = 1, Name = "Tecnología" };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            // Act
            var result = await _handler.Handle(new GetCategoryById(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryDto>();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Tecnología");
        }

        [Fact]
        public async Task Handle_CategoryNotFound_ReturnsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CategoryEntity?)null);

            // Act
            var result = await _handler.Handle(new GetCategoryById(99), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_CategoryExists_MapsAllFieldsCorrectly()
        {
            // Arrange
            var category = new CategoryEntity { Id = 5, Name = "Deportes" };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            // Act
            var result = await _handler.Handle(new GetCategoryById(5), CancellationToken.None);

            // Assert
            result!.Id.Should().Be(category.Id);
            result.Name.Should().Be(category.Name);
        }

        [Fact]
        public async Task Handle_Always_CallsGetByIdAsyncWithCorrectId()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CategoryEntity?)null);

            // Act
            await _handler.Handle(new GetCategoryById(3), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}