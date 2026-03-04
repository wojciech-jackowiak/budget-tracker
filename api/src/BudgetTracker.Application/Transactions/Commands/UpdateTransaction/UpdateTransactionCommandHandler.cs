using BudgetTracker.Application.Common.Exceptions;
using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Unit>
    {
        private readonly IBudgetTrackerDbContext _context;

        public UpdateTransactionCommandHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.Id);

            if (transaction.UserId != request.UserId)
                throw new UnauthorizedAccessException("You can only update your own transactions");

            transaction.Update(
                categoryId: request.CategoryId,
                amount: request.Amount,
                type: request.Type,
                description: request.Description,
                date: request.Date
            );
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
