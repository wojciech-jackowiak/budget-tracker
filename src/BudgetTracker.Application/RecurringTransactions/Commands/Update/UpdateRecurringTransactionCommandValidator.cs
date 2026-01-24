using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Commands.Update
{
    public class UpdateRecurringTransactionCommandValidator : AbstractValidator<UpdateRecurringTransactionCommand>
    {
        public UpdateRecurringTransactionCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Recurring transaction ID must be greater than 0");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0")
                .LessThanOrEqualTo(1_000_000).WithMessage("Amount cannot exceed 1,000,000");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.DayOfMonth)
                .GreaterThanOrEqualTo(1).WithMessage("Day of month must be at least 1")
                .LessThanOrEqualTo(31).WithMessage("Day of month cannot exceed 31");
        }
    }
}
