using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Transactions.Queries.GetTransactions
{
    public record TransactionDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public decimal Amount { get; init; }
        public string Description { get; init; } = string.Empty;
        public DateTime Date { get; init; }
        public string MonthYear { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty; 

        public int CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;
        public string CategoryIcon { get; init; } = string.Empty;
        public string CategoryColor { get; init; } = string.Empty;

        public decimal SignedAmount => Type == "Income" ? Amount : -Amount;

        public bool IsFromRecurring { get; init; }
        public int? RecurringTransactionId { get; init; }
    }
}
