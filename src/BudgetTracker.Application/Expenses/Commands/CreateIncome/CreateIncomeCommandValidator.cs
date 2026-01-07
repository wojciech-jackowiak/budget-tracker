using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Expenses.Commands.CreateIncome
{ 
    public class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
    {
        public CreateIncomeCommandValidator()
        {
            RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero")
            .LessThanOrEqualTo(1000000)
            .WithMessage("Amount exceeds maximum allowed value");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required")
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                .WithMessage("Date cannot be in the future");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("Valid user is required");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Valid category is required");
        }
    }
}
