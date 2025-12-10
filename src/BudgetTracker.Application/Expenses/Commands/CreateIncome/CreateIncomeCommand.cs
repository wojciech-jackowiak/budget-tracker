using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Expenses.Commands.CreateIncome
{    
    public class CreateIncomeCommand : IRequest<int>
    {
        public decimal Amount { get; set; }
        public required string Description { get; set; }
        public DateTime Date { get; set; }
        public int? CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
