using BudgetTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Commands.Create
{
    public record CreateRecurringTransactionCommand : IRequest<int>
    {
        public int UserId { get; init; }
        public required int CategoryId { get; init; }
        public required decimal Amount { get; init; }
        public required TransactionType Type { get; init; }
        public required string Description { get; init; }
        public required int DayOfMonth { get; init; } 
        public required DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public FrequencyType Frequency { get; init; } = FrequencyType.Monthly;
    }
}
