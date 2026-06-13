using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.DeleteCategory;
using Application.Features.Categories.Commands.UpdateCategory;
using Application.Validators.Category;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Tests.Category.Validators
{
    public class CreateCategoryCommandValidatorTests
    {
        private readonly CreateCategoryCommandValidator _validator;

        public CreateCategoryCommandValidatorTests()
        {
            _validator = new CreateCategoryCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidName_PassesValidation()
        {
            // Arrange
            var command = new CreateCategoryCommand("Tecnología");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_EmptyName_FailsValidation()
        {
            // Arrange
            var command = new CreateCategoryCommand("");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("El nombre de la categoría es requerido.");
        }

        [Fact]
        public async Task Validate_NameExceeds100Chars_FailsValidation()
        {
            // Arrange
            var longName = new string('a', 101);
            var command = new CreateCategoryCommand(longName);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("La categoría no debe exceder los 100 caracteres.");
        }

        [Fact]
        public async Task Validate_NameExactly100Chars_PassesValidation()
        {
            // Arrange
            var name = new string('a', 100);
            var command = new CreateCategoryCommand(name);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }
    }

    public class UpdateCategoryCommandValidatorTests
    {
        private readonly UpdateCategoryCommandValidator _validator;

        public UpdateCategoryCommandValidatorTests()
        {
            _validator = new UpdateCategoryCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidCommand_PassesValidation()
        {
            // Arrange
            var command = new UpdateCategoryCommand(1, "Nombre Actualizado");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_IdIsZero_FailsValidation()
        {
            // Arrange
            var command = new UpdateCategoryCommand(0, "Nombre");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("El identificador de la categoría es requerido.");
        }

        [Fact]
        public async Task Validate_IdIsNegative_FailsValidation()
        {
            // Arrange
            var command = new UpdateCategoryCommand(-1, "Nombre");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public async Task Validate_EmptyName_FailsValidation()
        {
            // Arrange
            var command = new UpdateCategoryCommand(1, "");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("El nombre de la categoría es requerido.");
        }

        [Fact]
        public async Task Validate_NameExceeds100Chars_FailsValidation()
        {
            // Arrange
            var longName = new string('a', 101);
            var command = new UpdateCategoryCommand(1, longName);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("La categoría no debe exceder los 100 caracteres.");
        }
    }

    public class DeleteCategoryCommandValidatorTests
    {
        private readonly DeleteCategoryCommandValidator _validator;

        public DeleteCategoryCommandValidatorTests()
        {
            _validator = new DeleteCategoryCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidId_PassesValidation()
        {
            // Arrange
            var command = new DeleteCategoryCommand(1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_IdIsZero_FailsValidation()
        {
            // Arrange
            var command = new DeleteCategoryCommand(0);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("El identificador de la categoría es requerido.");
        }

        [Fact]
        public async Task Validate_IdIsNegative_FailsValidation()
        {
            // Arrange
            var command = new DeleteCategoryCommand(-5);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}