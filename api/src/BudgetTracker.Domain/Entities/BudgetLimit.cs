using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Entities
{
    public class BudgetLimit : BaseEntity
    {
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;

        public int CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        public string MonthYear { get; private set; }
        public decimal LimitAmount { get; private set; }

        private BudgetLimit()
        {
            MonthYear = string.Empty;
        }

        public static BudgetLimit Create(int userId, int categoryId, string monthYear, decimal limitAmount)
        {
            ValidateMonthYear(monthYear);
            ValidateLimitAmount(limitAmount);

            return new BudgetLimit
            {
                UserId = userId,
                CategoryId = categoryId,
                MonthYear = monthYear,
                LimitAmount = limitAmount
            };
        }

        public void UpdateLimit(decimal newLimitAmount)
        {
            ValidateLimitAmount(newLimitAmount);
            LimitAmount = newLimitAmount;
            MarkAsModified();
        }

        private static void ValidateMonthYear(string monthYear)
        {
            var pattern = @"^\d{4}-\d{2}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(monthYear, pattern))
                throw new DomainException("Invalid month-year format (expected yyyy-MM)");
        }

        private static void ValidateLimitAmount(decimal limitAmount)
        {
            if (limitAmount <= 0)
                throw new DomainException("Budget limit must be greater than zero");
        }
    }
}
