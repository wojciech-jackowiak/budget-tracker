using BudgetTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Transactions.Commands.UpdateTransaction
{
    public record UpdateTransactionCommand : IRequest<Unit>
    {
        public required int Id { get; init; }              
        public int UserId { get; init; }          
        public required int CategoryId { get; init; }
        public required decimal Amount { get; init; }
        public required TransactionType Type { get; init; }
        public required string Description { get; init; }
        public required DateTime Date { get; init; }
    }
}
