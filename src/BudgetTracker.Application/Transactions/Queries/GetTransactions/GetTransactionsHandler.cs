using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Application.Transactions.Queries.GetTransactions
{
    public class GetTransactionsHandler : IRequestHandler<GetTransactionsQuery, List<TransactionDto>>
    {
        private readonly IBudgetTrackerDbContext _context;

        public GetTransactionsHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<List<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == request.UserId);

            // Opcjonalne filtry
            if (!string.IsNullOrEmpty(request.MonthYear))
            {
                query = query.Where(t => t.MonthYear == request.MonthYear);
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == request.CategoryId.Value);
            }

            if (!string.IsNullOrEmpty(request.Type))
            {
                var typeEnum = Enum.Parse<TransactionType>(request.Type);
                query = query.Where(t => t.Type == typeEnum);
            }

            var transactions = await query
                .OrderByDescending(t => t.Date)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Amount = t.Amount,
                    Description = t.Description,
                    Date = t.Date,
                    MonthYear = t.MonthYear,
                    Type = t.Type.ToString(),
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name,
                    CategoryIcon = t.Category.IconName,
                    CategoryColor = t.Category.ColorCode,
                    IsFromRecurring = t.RecurringTransactionId.HasValue,
                    RecurringTransactionId = t.RecurringTransactionId
                })
                .ToListAsync(cancellationToken);

            return transactions;
        }
    }
}
