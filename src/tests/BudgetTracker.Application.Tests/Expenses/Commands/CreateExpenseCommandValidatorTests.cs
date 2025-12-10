using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;

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
            var command = new CreateExpenseCommand
            {
                Amount = 50.00m,
                Description = "Dinner",
                Date = DateTime.UtcNow,
                UserId = 1
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Fact]
        public void Validate_WithNegativeAmount_ShouldHaveErorror()
        {
            var command = new CreateExpenseCommand
            {
                Amount = -10.00m,
                Description = "Dinner",
                Date = DateTime.UtcNow,
                UserId = 1
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Amount);
        }
        [Fact]
        public void Validate_WithEmptyDescription_ShouldHaveError()
        {
            var command = new CreateExpenseCommand
            {
                Amount = 20.00m,
                Description = "",
                Date = DateTime.UtcNow,
                UserId = 1
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Description);
        }
        [Fact]
        public void Validate_WithTooLongDescription_ShouldHaveError()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 100m,
                Description = new string('a', 201),
                Date = DateTime.UtcNow,
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
                UserId = 0
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }
    }
}
