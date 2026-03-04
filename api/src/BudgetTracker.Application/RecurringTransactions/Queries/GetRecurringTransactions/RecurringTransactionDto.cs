using BudgetTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Queries.GetRecurringTransactions
{
    public record RecurringTransactionDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public int CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;
        public string CategoryIcon { get; init; } = string.Empty;
        public string CategoryColor { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public TransactionType Type { get; init; }
        public string Description { get; init; } = string.Empty;
        public FrequencyType Frequency { get; init; }
        public int DayOfMonth { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public bool IsActive { get; init; }
        public string? LastProcessedMonth { get; init; }
        public int GeneratedTransactionsCount { get; init; } 
        public DateTime CreatedAt { get; init; }
    }
}
