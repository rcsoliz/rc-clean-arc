using Application.Features.Categories.Commands.DeleteCategory;
using Application.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.Category.Commands
{
    public class DeleteCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly DeleteCategoryCommandHandler _handler;

        public DeleteCategoryCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICategoryRepository>();
            _handler = new DeleteCategoryCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_CategoryExists_ReturnsTrue()
        {
            // Arrange
            var command = new DeleteCategoryCommand(1);

            _repositoryMock
                .Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_CategoryNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new DeleteCategoryCommand(99);

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
            var command = new DeleteCategoryCommand(5);

            _repositoryMock
                .Setup(r => r.DeleteAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.DeleteAsync(5, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}