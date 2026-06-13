using Application.Features.Posts.Commands.CreatePost;
using Application.Validators.Posts;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Tests.Posts.Validators
{
    public class CreatePostCommandValidatorTests
    {
        private readonly CreatePostCommandValidator _validator;

        public CreatePostCommandValidatorTests()
        {
            _validator = new CreatePostCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidCommand_PassesValidation()
        {
            // Arrange
            var command = new CreatePostCommand("Contenido válido del post", 1, null, new List<int>());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_EmptyContent_FailsValidation()
        {
            // Arrange
            var command = new CreatePostCommand("", 1, null, new List<int>());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PostContent)
                .WithErrorMessage("El contenido del post es requerido.");
        }

        [Fact]
        public async Task Validate_ContentExceeds1000Chars_FailsValidation()
        {
            // Arrange
            var longContent = new string('a', 1001);
            var command = new CreatePostCommand(longContent, 1, null, new List<int>());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PostContent)
                .WithErrorMessage("El contenido no debe exceder los 1000 caracteres.");
        }

        [Fact]
        public async Task Validate_ContentExactly1000Chars_PassesValidation()
        {
            // Arrange
            var content = new string('a', 1000);
            var command = new CreatePostCommand(content, 1, null, new List<int>());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PostContent);
        }

        [Fact]
        public async Task Validate_UserIdIsZero_FailsValidation()
        {
            // Arrange
            var command = new CreatePostCommand("Contenido válido", 0, null, new List<int>());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("El identificador del usuario es requerido.");
        }

        [Fact]
        public async Task Validate_UserIdIsNegative_FailsValidation()
        {
            // Arrange
            var command = new CreatePostCommand("Contenido válido", -1, null, new List<int>());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public async Task Validate_WithImageUrl_PassesValidation()
        {
            // Arrange
            var command = new CreatePostCommand("Contenido válido", 1, "https://img.com/foto.jpg", new List<int>());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}