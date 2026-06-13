using Application.Features.Users.Commands.CreateUser;
using Application.Validators.Users;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Tests.Users.Validators
{
    public class CreateUserCommandValidatorTests
    {
        private readonly CreateUserCommandValidator _validator;

        public CreateUserCommandValidatorTests()
        {
            _validator = new CreateUserCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidCommand_PassesValidation()
        {
            // Arrange
            var command = new CreateUserCommand("carlos", "carlos@email.com", "Password123");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_EmptyUsername_FailsValidation()
        {
            // Arrange
            var command = new CreateUserCommand("", "carlos@email.com", "Password123");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username)
                .WithErrorMessage("El nombre de usuario es obligatorio.");
        }

        [Fact]
        public async Task Validate_EmptyEmail_FailsValidation()
        {
            // Arrange
            var command = new CreateUserCommand("carlos", "", "Password123");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("El email es obligatorio.");
        }

        [Fact]
        public async Task Validate_InvalidEmailFormat_FailsValidation()
        {
            // Arrange
            var command = new CreateUserCommand("carlos", "no-es-un-email", "Password123");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("El email no es válido.");
        }

        [Fact]
        public async Task Validate_EmptyPassword_FailsValidation()
        {
            // Arrange
            var command = new CreateUserCommand("carlos", "carlos@email.com", "");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("La contraseña es obligatoria.");
        }

        [Fact]
        public async Task Validate_PasswordLessThan6Chars_FailsValidation()
        {
            // Arrange
            var command = new CreateUserCommand("carlos", "carlos@email.com", "12345");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("La contraseña debe tener al menos 6 caracteres.");
        }

        [Fact]
        public async Task Validate_PasswordExactly6Chars_PassesValidation()
        {
            // Arrange
            var command = new CreateUserCommand("carlos", "carlos@email.com", "123456");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async Task Validate_MultipleErrors_ReturnsAllErrors()
        {
            // Arrange — comando completamente inválido
            var command = new CreateUserCommand("", "no-es-email", "123");

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username);
            result.ShouldHaveValidationErrorFor(x => x.Email);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}