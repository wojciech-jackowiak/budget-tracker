using MediatR;

namespace BudgetTracker.Application.Transactions.Queries.GetTransactions
{
    public record GetTransactionsQuery : IRequest<List<TransactionDto>>
    {
        public required int UserId { get; init; }
        public string? MonthYear { get; init; }
        public int? CategoryId { get; init; } 
        public string? Type { get; init; } 
    }
}
