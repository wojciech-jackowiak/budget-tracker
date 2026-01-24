using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public DateTime Date { get; private set; }
        public string MonthYear { get; private set; }
        public TransactionType Type { get; private set; }

        public int CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        public int UserId { get; private set; }
        public User User { get; private set; } = null!;
        public bool IsFromRecurring { get; private set; }
        public int? RecurringTransactionId { get; private set; }
        public RecurringTransaction? RecurringTransaction { get; private set; }

        private Transaction()
        {
            Description = string.Empty;
            MonthYear = string.Empty;
        }

        public static Transaction Create(
            int userId,
            int categoryId,
            decimal amount,
            TransactionType type,
            string description,
            DateTime date,
            bool isFromRecurring = false,         
            int? recurringTransactionId = null)
        {
            ValidateAmount(amount);
            ValidateDescription(description);

            return new Transaction
            {
                UserId = userId,
                CategoryId = categoryId,
                Amount = amount,
                Type = type,
                Description = description,
                Date = date,
                MonthYear = date.ToString("yyyy-MM"),
                IsFromRecurring = isFromRecurring,
                RecurringTransactionId = recurringTransactionId                
            };
        }

        public static Transaction CreateFromRecurring(
            RecurringTransaction recurringTransaction,
            DateTime date)
        {
            return new Transaction
            {
                UserId = recurringTransaction.UserId,
                CategoryId = recurringTransaction.CategoryId,
                Amount = recurringTransaction.Amount,
                Type = recurringTransaction.Type,
                Description = recurringTransaction.Description,
                Date = date,
                MonthYear = date.ToString("yyyy-MM"),
                RecurringTransactionId = recurringTransaction.Id
            };
        }

        public void Update(decimal amount, int categoryId, string description, DateTime date, TransactionType type)
        {
            if (RecurringTransactionId.HasValue)
                throw new DomainException("Cannot edit recurring-generated transaction");

            ValidateAmount(amount);
            ValidateDescription(description);

            Amount = amount;
            CategoryId = categoryId;
            Description = description;
            Type = type;
            Date = date;
            MonthYear = date.ToString("yyyy-MM");
            MarkAsModified();
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
                throw new DomainException("Amount must be greater than zero");

            if (amount > 1_000_000_000)
                throw new DomainException("Amount exceeds maximum allowed value");
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description cannot be empty");

            if (description.Length > 500)
                throw new DomainException("Description too long (max 500 characters)");
        }
    }
}
