using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public required string Description { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
