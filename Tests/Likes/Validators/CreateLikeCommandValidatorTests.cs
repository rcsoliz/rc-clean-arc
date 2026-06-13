using Application.Features.Likes.Commands.CreateLike;
using Application.Features.Likes.Commands.DeleteLike;
using Application.Features.Likes.Commands.UpdateLike;
using Application.Validators.Like;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Tests.Likes.Validators
{
    public class CreateLikeCommandValidatorTests
    {
        private readonly CreateLikeCommandValidator _validator;

        public CreateLikeCommandValidatorTests()
        {
            _validator = new CreateLikeCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidCommand_PassesValidation()
        {
            // Arrange
            var command = new CreateLikeCommand(1, 1, null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_UserIdIsZero_FailsValidation()
        {
            // Arrange
            var command = new CreateLikeCommand(0, 1, null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("El usuario es requerido");
        }

        [Fact]
        public async Task Validate_PostIdIsZero_FailsValidation()
        {
            // Arrange
            var command = new CreateLikeCommand(1, 0, null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PostId)
                .WithErrorMessage("El post es requerido");
        }

        [Fact]
        public async Task Validate_WithCommentId_PassesValidation()
        {
            // Arrange
            var command = new CreateLikeCommand(1, 1, 5);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

    public class UpdateLikeCommandValidatorTests
    {
        private readonly UpdateLikeCommandValidator _validator;

        public UpdateLikeCommandValidatorTests()
        {
            _validator = new UpdateLikeCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidCommand_PassesValidation()
        {
            // Arrange
            var command = new UpdateLikeCommand(1, 1, 1, 1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_IdIsZero_FailsValidation()
        {
            // Arrange
            var command = new UpdateLikeCommand(0, 1, 1, 1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("El Id del like debe ser mayor que 0.");
        }

        [Fact]
        public async Task Validate_UserIdIsZero_FailsValidation()
        {
            // Arrange
            var command = new UpdateLikeCommand(1, 0, 1, 1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("El Id del usuario debe ser mayor que 0.");
        }

        [Fact]
        public async Task Validate_PostIdIsZero_FailsValidation()
        {
            // Arrange
            var command = new UpdateLikeCommand(1, 1, 0, 1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PostId)
                .WithErrorMessage("El Id del post debe ser mayor que 0.");
        }

        [Fact]
        public async Task Validate_CommentIdIsZero_FailsValidation()
        {
            // Arrange
            var command = new UpdateLikeCommand(1, 1, 1, 0);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CommentId)
                .WithErrorMessage("El Id del comentario debe ser mayor que 0.");
        }
    }

    public class DeleteLikeCommandValidatorTests
    {
        private readonly DeleteLikeCommandValidator _validator;

        public DeleteLikeCommandValidatorTests()
        {
            _validator = new DeleteLikeCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidId_PassesValidation()
        {
            // Arrange
            var command = new DeleteLikeCommand(1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_IdIsZero_FailsValidation()
        {
            // Arrange
            var command = new DeleteLikeCommand(0);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("El Id del like debe ser mayor que 0.");
        }

        [Fact]
        public async Task Validate_IdIsNegative_FailsValidation()
        {
            // Arrange
            var command = new DeleteLikeCommand(-1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}