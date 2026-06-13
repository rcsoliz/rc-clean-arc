using Application.Features.Comments.Commands.CreateComment;
using Application.Features.Comments.Commands.DeleteComment;
using Application.Validators.Comments;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Tests.Comments.Validators
{
    public class CreateCommentCommandValidatorTests
    {
        private readonly CreateCommentCommandValidator _validator;

        public CreateCommentCommandValidatorTests()
        {
            _validator = new CreateCommentCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidCommand_PassesValidation()
        {
            // Arrange
            var command = new CreateCommentCommand("Gran artículo!", 1, 1, null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_EmptyContent_FailsValidation()
        {
            // Arrange
            var command = new CreateCommentCommand("", 1, 1, null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CommentContent)
                .WithErrorMessage("El contenido del comentario es requerido.");
        }

        [Fact]
        public async Task Validate_ContentExceeds500Chars_FailsValidation()
        {
            // Arrange
            var longContent = new string('a', 501);
            var command = new CreateCommentCommand(longContent, 1, 1, null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CommentContent)
                .WithErrorMessage("El comentario no debe exceder los 500 caracteres.");
        }

        [Fact]
        public async Task Validate_ContentExactly500Chars_PassesValidation()
        {
            // Arrange
            var content = new string('a', 500);
            var command = new CreateCommentCommand(content, 1, 1, null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.CommentContent);
        }

        [Fact]
        public async Task Validate_PostIdIsZero_FailsValidation()
        {
            // Arrange
            var command = new CreateCommentCommand("Contenido válido", 1, 0, null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PostId)
                .WithErrorMessage("El identificador del post es requerido.");
        }

        [Fact]
        public async Task Validate_UserIdIsZero_FailsValidation()
        {
            // Arrange
            var command = new CreateCommentCommand("Contenido válido", 0, 1, null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("El identificador del usuario es requerido.");
        }

        [Fact]
        public async Task Validate_WithValidParentCommentId_PassesValidation()
        {
            // Arrange
            var command = new CreateCommentCommand("Respuesta", 1, 1, 5);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

    public class DeleteCommentCommandValidatorTests
    {
        private readonly DeleteCommentCommandValidator _validator;

        public DeleteCommentCommandValidatorTests()
        {
            _validator = new DeleteCommentCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidId_PassesValidation()
        {
            // Arrange
            var command = new DeleteCommentCommand(1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_IdIsZero_FailsValidation()
        {
            // Arrange
            var command = new DeleteCommentCommand(0);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("El identificador del comentario es requerido.");
        }

        [Fact]
        public async Task Validate_IdIsNegative_FailsValidation()
        {
            // Arrange
            var command = new DeleteCommentCommand(-1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}