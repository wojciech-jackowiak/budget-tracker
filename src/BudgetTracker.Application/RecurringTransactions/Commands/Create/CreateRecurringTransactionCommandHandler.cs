using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Commands.Create
{
    public class CreateRecurringTransactionCommandHandler : IRequestHandler<CreateRecurringTransactionCommand, int>
    {
        private readonly IBudgetTrackerDbContext _context;

        public CreateRecurringTransactionCommandHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateRecurringTransactionCommand request, CancellationToken cancellationToken)
        {
            //CreateInfinite vs CreateFixedTerm
            RecurringTransaction recurring;

            if (request.EndDate.HasValue)
            {
                // Fixed term
                recurring = RecurringTransaction.CreateFixedTerm(
                    userId: request.UserId,
                    categoryId: request.CategoryId,
                    amount: request.Amount,
                    type: request.Type,
                    description: request.Description,
                    startDate: request.StartDate,
                    endDate: request.EndDate.Value,
                    dayOfMonth: request.DayOfMonth,
                    frequency: request.Frequency
                );
            }
            else
            {
                // Infinite
                recurring = RecurringTransaction.CreateInfinite(
                    userId: request.UserId,
                    categoryId: request.CategoryId,
                    amount: request.Amount,
                    type: request.Type,
                    description: request.Description,
                    startDate: request.StartDate,
                    dayOfMonth: request.DayOfMonth,
                    frequency: request.Frequency
                );
            }

            _context.RecurringTransactions.Add(recurring);
            await _context.SaveChangesAsync(cancellationToken);

            return recurring.Id;
        }
    }
}
