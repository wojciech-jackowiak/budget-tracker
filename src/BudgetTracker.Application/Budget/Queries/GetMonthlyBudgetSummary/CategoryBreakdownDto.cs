using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Budget.Queries.GetMonthlyBudgetSummary
{
    public class CategoryBreakdownDto
    {
        public int CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;
        public string CategoryIcon { get; init; } = string.Empty;
        public string CategoryColor { get; init; } = string.Empty;
        public decimal Income { get; init; }
        public decimal Expenses { get; init; }
        public decimal Net => Income - Expenses;
        public int TransactionCount { get; init; }
        public decimal? BudgetLimit { get; init; }

        public decimal? PercentageOfLimit => BudgetLimit.HasValue && BudgetLimit > 0 ? (Expenses / BudgetLimit.Value) * 100: null;
        public bool IsOverBudget => BudgetLimit.HasValue && Expenses > BudgetLimit.Value;
    }
}
