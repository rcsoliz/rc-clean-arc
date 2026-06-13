using Application.DTOs;
using Application.Features.Categories.Commands.CreateCategory;
using Application.Interfaces;
using CategoryEntity = Core.Entities.Category;
using FluentAssertions;
using Moq;

namespace Tests.Category.Commands
{
    public class CreateCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly CreateCategoryCommandHandler _handler;

        public CreateCategoryCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICategoryRepository>();
            _handler = new CreateCategoryCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsCategoryDto()
        {
            // Arrange
            var command = new CreateCategoryCommand("Tecnología");

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<CategoryEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryDto>();
            result.Name.Should().Be("Tecnología");
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsAddAsyncOnce()
        {
            // Arrange
            var command = new CreateCategoryCommand("Deportes");

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<CategoryEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(It.Is<CategoryEntity>(c => c.Name == "Deportes"), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_CategoryDtoHasCorrectName()
        {
            // Arrange
            var command = new CreateCategoryCommand("Ciencia");

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<CategoryEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Name.Should().Be(command.Name);
        }
    }
}