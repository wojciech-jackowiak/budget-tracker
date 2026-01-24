using BudgetTracker.Application.Common.Exceptions;
using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Unit>
    {
        private readonly IBudgetTrackerDbContext _context;

        public DeleteTransactionCommandHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.Id);

            if (transaction.UserId != request.UserId)
                throw new UnauthorizedAccessException("You can only delete your own transactions");

            if (transaction.IsFromRecurring)
                throw new DomainException("Cannot delete recurring-generated transaction. Delete the recurring transaction instead.");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
