using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
    {
        public DeleteTransactionCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Transaction ID must be greater than 0");
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0");
        }
    }
}