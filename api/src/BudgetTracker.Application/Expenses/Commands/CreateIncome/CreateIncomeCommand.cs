using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Expenses.Commands.CreateIncome
{    
    public record CreateIncomeCommand : IRequest<int>
    {
        public int UserId { get; init; }
        public required int CategoryId { get; init; }
        public required decimal Amount { get; init; }
        public required string Description { get; init; }
        public DateTime Date { get; init; } = DateTime.UtcNow;
    }
}
