using Application.Features.Categories.Commands.UpdateCategory;
using Application.Interfaces;
using CategoryEntity = Core.Entities.Category;
using FluentAssertions;
using Moq;

namespace Tests.Category.Commands
{
    public class UpdateCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly UpdateCategoryCommandHandler _handler;

        public UpdateCategoryCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICategoryRepository>();
            _handler = new UpdateCategoryCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_CategoryExists_ReturnsTrue()
        {
            // Arrange
            var command = new UpdateCategoryCommand(1, "Tecnología Actualizada");
            var existingCategory = new CategoryEntity { Id = 1, Name = "Tecnología" };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<CategoryEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_CategoryNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateCategoryCommand(99, "No Existe");

            _repositoryMock
                .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CategoryEntity?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_CategoryNotFound_NeverCallsUpdateAsync()
        {
            // Arrange
            var command = new UpdateCategoryCommand(99, "No Existe");

            _repositoryMock
                .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CategoryEntity?)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<CategoryEntity>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_CategoryExists_UpdatesNameCorrectly()
        {
            // Arrange
            var command = new UpdateCategoryCommand(1, "Nombre Nuevo");
            var existingCategory = new CategoryEntity { Id = 1, Name = "Nombre Viejo" };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<CategoryEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.UpdateAsync(It.Is<CategoryEntity>(c => c.Name == "Nombre Nuevo"), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}