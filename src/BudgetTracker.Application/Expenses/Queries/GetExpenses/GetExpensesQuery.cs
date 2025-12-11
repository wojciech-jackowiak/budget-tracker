using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Expenses.Queries.GetExpenses
{
    public class GetExpensesQuery : IRequest<List<ExpenseDto>>
    {
        public int UserId { get; set; }
    }
}
