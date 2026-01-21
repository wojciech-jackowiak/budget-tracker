using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Application.Transactions.Queries.GetTransactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace BudgetTracker.Application.Transactions.Queries.GetTransactionsById
{
    public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto?>
    {
        private readonly IBudgetTrackerDbContext _context;

        public GetTransactionByIdHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionDto?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Id == request.Id)
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
                .FirstOrDefaultAsync(cancellationToken);

            return transaction;
        }
    }
}
