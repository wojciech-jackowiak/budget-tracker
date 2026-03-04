using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Commands.Pause
{
    public class PauseRecurringTransactionCommandValidator : AbstractValidator<PauseRecurringTransactionCommand>
    {
        public PauseRecurringTransactionCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Recurring transaction ID must be greater than 0");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0");
        }
    }
}
