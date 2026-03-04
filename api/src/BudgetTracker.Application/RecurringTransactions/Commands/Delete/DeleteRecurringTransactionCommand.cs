using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Commands.Delete
{
    public record DeleteRecurringTransactionCommand : IRequest<Unit>
    {
        public required int Id { get; init; }
        public int UserId { get; init; }
    }
}
