using BudgetTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.RecurringTransactions.Queries.GetRecurringTransactions
{
    public record GetRecurringTransactionsQuery : IRequest<List<RecurringTransactionDto>>
    {
        public required int UserId { get; init; }  
        public bool? IsActive { get; init; } 
        public TransactionType? Type { get; init; } 
    }
}
