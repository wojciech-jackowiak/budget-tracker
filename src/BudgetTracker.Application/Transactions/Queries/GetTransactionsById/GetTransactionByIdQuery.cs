using BudgetTracker.Application.Transactions.Queries.GetTransactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Transactions.Queries.GetTransactionsById
{
    public record GetTransactionByIdQuery : IRequest<TransactionDto?>
    {
        public required int Id { get; init; }
    }
}
