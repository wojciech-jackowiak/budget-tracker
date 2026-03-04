using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Budget.Queries.GetMonthlyBudgetSummary
{
    public class GetMonthlyBudgetSummaryHandler: IRequestHandler<GetMonthlyBudgetSummaryQuery, BudgetSummaryDto>
    {
        private readonly IBudgetTrackerDbContext _context;

        public GetMonthlyBudgetSummaryHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<BudgetSummaryDto> Handle(GetMonthlyBudgetSummaryQuery request,CancellationToken cancellationToken)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == request.UserId && t.MonthYear == request.MonthYear)
                .ToListAsync(cancellationToken);

            var budgetLimits = await _context.BudgetLimits
                .Where(bl => bl.UserId == request.UserId && bl.MonthYear == request.MonthYear)
                .ToListAsync(cancellationToken);

            var totalIncome = transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var totalExpenses = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            var categoryBreakdown = transactions
            .GroupBy(t => t.Category)
            .Select(group =>
            {
                var budgetLimit = budgetLimits
                    .FirstOrDefault(bl => bl.CategoryId == group.Key.Id);

                var categoryIncome = group
                    .Where(t => t.Type == TransactionType.Income)
                    .Sum(t => t.Amount);

                var categoryExpenses = group
                    .Where(t => t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount);

                return new CategoryBreakdownDto
                {
                    CategoryId = group.Key.Id,
                    CategoryName = group.Key.Name,
                    CategoryIcon = group.Key.IconName,
                    CategoryColor = group.Key.ColorCode,
                    Income = categoryIncome,
                    Expenses = categoryExpenses,
                    TransactionCount = group.Count(),
                    BudgetLimit = budgetLimit?.LimitAmount
                };
            })
            .OrderByDescending(c => Math.Abs(c.Net)) 
            .ToList();

            return new BudgetSummaryDto
            {
                UserId = request.UserId,
                MonthYear = request.MonthYear,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                TransactionCount = transactions.Count,
                CategoryBreakdown = categoryBreakdown
            };
        }
    }
}
