using BudgetTracker.Application.Common.Exceptions;
using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Commands.Resume
{
    public class ResumeRecurringTransactionCommandHandler : IRequestHandler<ResumeRecurringTransactionCommand, Unit>
    {
        private readonly IBudgetTrackerDbContext _context;

        public ResumeRecurringTransactionCommandHandler(IBudgetTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ResumeRecurringTransactionCommand request, CancellationToken cancellationToken)
        {
            var recurring = await _context.RecurringTransactions
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (recurring == null)
                throw new NotFoundException(nameof(RecurringTransaction), request.Id);

            if (recurring.UserId != request.UserId)
                throw new UnauthorizedAccessException("You can only resume your own recurring transactions");

            recurring.Reactivate();
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
