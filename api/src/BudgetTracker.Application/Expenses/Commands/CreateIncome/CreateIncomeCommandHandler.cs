using BudgetTracker.Application.Common.Exceptions;
using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Application.Expenses.Commands.CreateIncome
{
    public class CreateIncomeHandler : IRequestHandler<CreateIncomeCommand, int>
    {
        private readonly IBudgetTrackerDbContext _context;

        public CreateIncomeHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            var income = Transaction.Create(
                userId: request.UserId,
                categoryId: request.CategoryId,
                amount: request.Amount,
                type: TransactionType.Income,
                description: request.Description,
                date: request.Date
            );

            _context.Transactions.Add(income);
            await _context.SaveChangesAsync(cancellationToken);

            return income.Id;
        }
    }
}
