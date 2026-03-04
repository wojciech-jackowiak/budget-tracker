using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Budget.Queries.GetMonthlyBudgetSummary
{
    public class BudgetSummaryDto
    {
        public int UserId { get; init; }
        public string MonthYear { get; init; } = string.Empty;

        public decimal TotalIncome { get; init; }
        public decimal TotalExpenses { get; init; }
        public decimal Net => TotalIncome - TotalExpenses;
        public decimal SavingsRate => TotalIncome > 0? (Net / TotalIncome) * 100 : 0;
        public int TransactionCount { get; init; }
        public List<CategoryBreakdownDto> CategoryBreakdown { get; init; } = new();
    }
}
