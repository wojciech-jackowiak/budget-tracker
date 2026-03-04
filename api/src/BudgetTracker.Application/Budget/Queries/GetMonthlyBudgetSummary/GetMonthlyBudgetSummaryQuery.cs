using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Budget.Queries.GetMonthlyBudgetSummary
{
    public record GetMonthlyBudgetSummaryQuery : IRequest<BudgetSummaryDto>
    {
        public required int UserId { get; init; }
        public required string MonthYear { get; init; }
    }
}
