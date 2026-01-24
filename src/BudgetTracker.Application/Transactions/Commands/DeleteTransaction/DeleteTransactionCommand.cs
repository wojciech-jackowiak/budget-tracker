using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Transactions.Commands.DeleteTransaction
{
    public record DeleteTransactionCommand : IRequest<Unit>
    {
        public required int Id { get; init; }
        public int UserId { get; init; } 
    }
}
