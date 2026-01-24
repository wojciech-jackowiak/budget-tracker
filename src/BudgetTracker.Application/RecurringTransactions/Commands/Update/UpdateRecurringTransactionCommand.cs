using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Commands.Update
{
    public record UpdateRecurringTransactionCommand : IRequest<Unit>
    {
        public required int Id { get; init; }
        public int UserId { get; init; }
        public required decimal Amount { get; init; }
        public required string Description { get; init; }
        public required int DayOfMonth { get; init; }
    }
}
