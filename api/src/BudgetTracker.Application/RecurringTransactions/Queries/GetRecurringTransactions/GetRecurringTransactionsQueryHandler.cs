using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Queries.GetRecurringTransactions
{
    public class GetRecurringTransactionsQueryHandler
    : IRequestHandler<GetRecurringTransactionsQuery, List<RecurringTransactionDto>>
    {
        private readonly IBudgetTrackerDbContext _context;

        public GetRecurringTransactionsQueryHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecurringTransactionDto>> Handle(
            GetRecurringTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.RecurringTransactions
                .Include(r => r.Category)
                .Include(r => r.GeneratedTransactions)  // Dla count
                .Where(r => r.UserId == request.UserId);

            if (request.IsActive.HasValue)
            {
                query = query.Where(r => r.IsActive == request.IsActive.Value);
            }

            if (request.Type.HasValue)
            {
                query = query.Where(r => r.Type == request.Type.Value);
            }

            query = query.OrderByDescending(r => r.StartDate);

            var result = await query
                .Select(r => new RecurringTransactionDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    CategoryId = r.CategoryId,
                    CategoryName = r.Category.Name,
                    CategoryIcon = r.Category.IconName,
                    CategoryColor = r.Category.ColorCode,
                    Amount = r.Amount,
                    Type = r.Type,
                    Description = r.Description,
                    Frequency = r.Frequency,
                    DayOfMonth = r.DayOfMonth,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    IsActive = r.IsActive,
                    LastProcessedMonth = r.LastProcessedMonth,
                    GeneratedTransactionsCount = r.GeneratedTransactions.Count,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
