using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using FluentValidation.TestHelper;

namespace BudgetTracker.Application.Tests.Expenses.Commands
{
    public class CreateExpenseCommandValidatorTests
    {
        private readonly CreateExpenseCommandValidator _validator;

        public CreateExpenseCommandValidatorTests()
        {
            _validator = new CreateExpenseCommandValidator();
        }

        [Fact]
        public void Validate_WithValidCommand_ShouldNotHaveErrors()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 50.00m,
                Description = "Dinner",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithZeroAmount_ShouldHaveError()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 0m,
                Description = "Dinner",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Amount);
        }

        [Fact]
        public void Validate_WithNegativeAmount_ShouldHaveError()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = -10.00m,
                Description = "Dinner",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Amount);
        }

        [Fact]
        public void Validate_WithEmptyDescription_ShouldHaveError()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 20.00m,
                Description = "",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Description);
        }

        [Fact]
        public void Validate_WithTooLongDescription_ShouldHaveError()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 100m,
                Description = new string('a', 501),
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Validate_WithFutureDate_ShouldHaveError()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 100m,
                Description = "Test",
                Date = DateTime.UtcNow.AddDays(7),
                CategoryId = 1,
                UserId = 1
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Date);
        }

        [Fact]
        public void Validate_WithZeroUserId_ShouldHaveError()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 100m,
                Description = "Test",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 0
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_WithZeroCategoryId_ShouldHaveError()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 100m,
                Description = "Test",
                Date = DateTime.UtcNow,
                CategoryId = 0, 
                UserId = 1
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategoryId);
        }

        [Fact]
        public void Validate_WithExcessiveAmount_ShouldHaveError()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 2000000000m, 
                Description = "Test",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }
    }
}
