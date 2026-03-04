using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Commands.Create
{
    public class CreateRecurringTransactionCommandValidator : AbstractValidator<CreateRecurringTransactionCommand>
    {
        public CreateRecurringTransactionCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0")
                .LessThanOrEqualTo(1_000_000).WithMessage("Amount cannot exceed 1,000,000");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.DayOfMonth)
                .GreaterThanOrEqualTo(1).WithMessage("Day of month must be at least 1")
                .LessThanOrEqualTo(31).WithMessage("Day of month cannot exceed 31");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID must be greater than 0");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("End date must be after start date");

        }
    }
}
