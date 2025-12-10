using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Expenses.Commands.CreateIncome
{   
    public class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateIncomeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                Amount = request.Amount,
                Description = request.Description,
                Date = request.Date,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
                Type = TransactionType.Income
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return transaction.Id;
        }
    }
}
