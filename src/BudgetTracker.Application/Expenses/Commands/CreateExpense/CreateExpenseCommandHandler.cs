using BudgetTracker.Application.Common.Exceptions;
using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace BudgetTracker.Application.Expenses.Commands.CreateExpense
{
    public class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, int>
    {
        private readonly IBudgetTrackerDbContext _context;

        public CreateExpenseHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            var expense = Transaction.Create(
                userId: request.UserId,
                categoryId: request.CategoryId,
                amount: request.Amount,
                type: TransactionType.Expense,
                description: request.Description,
                date: request.Date
            );

            _context.Transactions.Add(expense);
            await _context.SaveChangesAsync(cancellationToken);

            return expense.Id;
        }
    }
}
