using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Commands.Pause
{
    public record PauseRecurringTransactionCommand : IRequest<Unit>
    {
        public required int Id { get; init; }
        public int UserId { get; init; }
    }
}
