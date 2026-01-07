using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Entities
{
    public class RecurringTransaction : BaseEntity
    {
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;

        public int CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public string Description { get; private set; }
        public FrequencyType Frequency { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; } 
        public bool IsActive { get; private set; }
        public string? LastProcessedMonth { get; private set; } 

        private readonly List<Transaction> _generatedTransactions = new();
        public IReadOnlyCollection<Transaction> GeneratedTransactions => _generatedTransactions.AsReadOnly();

        private RecurringTransaction() { }

        public static RecurringTransaction CreateInfinite(
            int userId,
            int categoryId,
            decimal amount,
            TransactionType type,
            string description,
            DateTime startDate,
            FrequencyType frequency = FrequencyType.Monthly)
        {
            ValidateAmount(amount);
            ValidateDescription(description);

            return new RecurringTransaction
            {
                UserId = userId,
                CategoryId = categoryId,
                Amount = amount,
                Type = type,
                Description = description,
                StartDate = startDate,
                EndDate = null, // ← null = infinite!
                Frequency = frequency,
                IsActive = true
            };
        }

        public static RecurringTransaction CreateFixedTerm(
            int userId,
            int categoryId,
            decimal amount,
            TransactionType type,
            string description,
            DateTime startDate,
            DateTime endDate,
            FrequencyType frequency = FrequencyType.Monthly)
        {
            ValidateAmount(amount);
            ValidateDescription(description);

            if (endDate <= startDate)
                throw new DomainException("End date must be after start date");

            return new RecurringTransaction
            {
                UserId = userId,
                CategoryId = categoryId,
                Amount = amount,
                Type = type,
                Description = description,
                StartDate = startDate,
                EndDate = endDate, // ← fixed term!
                Frequency = frequency,
                IsActive = true
            };
        }

        public void MarkAsProcessed(string monthYear)
        {
            LastProcessedMonth = monthYear;

            if (EndDate.HasValue && DateTime.Parse($"{monthYear}-01") >= EndDate.Value)
            {
                IsActive = false;
            }

            MarkAsModified();
        }

        public void Deactivate()
        {
            IsActive = false;
            MarkAsModified();
        }

        public void Reactivate()
        {
            IsActive = true;
            MarkAsModified();
        }

        public bool ShouldProcessForMonth(string monthYear)
        {
            if (!IsActive) return false;

            var month = DateTime.Parse($"{monthYear}-01");

            // Czy jest przed StartDate?
            if (month < StartDate) return false;

            // Czy jest po EndDate?
            if (EndDate.HasValue && month > EndDate.Value) return false;

            // Czy już przetworzono?
            if (LastProcessedMonth == monthYear) return false;

            return true;
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
                throw new DomainException("Amount must be greater than zero");
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description cannot be empty");
        }
    }
}
