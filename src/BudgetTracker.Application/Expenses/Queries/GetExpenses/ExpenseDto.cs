using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BudgetTracker.Application.Expenses.Queries.GetExpenses
{
    public class ExpenseDto
    {
        public int Id { get; set; }   
        public decimal Amount { get; set; }
        public required string Description { get; set; }
        public DateTime Date { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
