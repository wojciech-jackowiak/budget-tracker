using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace BudgetTracker.Application.Expenses.Queries.GetExpenses
{
    public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, List<ExpenseDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetExpensesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ExpenseDto>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _context.Transactions
                .Where(t => t.UserId == request.UserId && t.Type == TransactionType.Expense)
                .Select(t => new ExpenseDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Description = t.Description,
                    Date = t.Date,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category != null ? t.Category.Name : null,
                })
                .ToListAsync(cancellationToken);

            return expenses;
        }
    }
}
